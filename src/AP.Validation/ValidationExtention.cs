using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;

namespace WebApplication2
{
    public static class ValidationExtention
    {
        private static IEnumerable<Type> ValidatorImplementations { get; set; }
        private static readonly Type ValidatorBaseType = typeof (AbstractValidator<>);
        private static Dictionary<Type, Tuple<MethodInfo, object>> CachedValidators { get; set; }

        static ValidationExtention()
        {
            var executingAssembly = AppDomain.CurrentDomain.GetAssemblies();
            var types = executingAssembly.SelectMany(a => a.GetTypes());

            ValidatorImplementations = types.Where(type => IsSubclassOfRawGeneric(ValidatorBaseType, type) && !type.IsAbstract);
            CachedValidators = new Dictionary<Type, Tuple<MethodInfo, object>>();
        }

        public static ValidationResult Validate<T>(this T model) where T : class
        {
            if (model == null)
            {
                throw new Exception("can't validate null object");
            }

            var modelType = typeof (T);

            var cachedValidator = GetValidatorInstanceEndMethodForType(modelType);
            var validatorInstance = cachedValidator.Item2;
            var validateMethodInfo = cachedValidator.Item1;
            var result = validateMethodInfo.Invoke(validatorInstance, new[] { model });
            if (result is ValidationResult)
            {
                return result as ValidationResult;
            }

            throw new Exception(string.Format("can't find suitable validator for type {0}", modelType));
        }

        //todo: we should have a typed wrapper around Tuple
        private static Tuple<MethodInfo, object> GetValidatorInstanceEndMethodForType(Type type)
        {
            if (CachedValidators.ContainsKey(type))
            {
                return CachedValidators[type];
            }
            var desiredValidatorType = ValidatorBaseType.MakeGenericType(type);

            var suitableValidatorType = ValidatorImplementations.Where(desiredValidatorType.IsAssignableFrom).FirstOrDefault();
            if (suitableValidatorType == null)
            {
                return null;
            }

            var validateMethodInfo = suitableValidatorType.GetMethod("Validate", new[] { type });

            var validatorInstance = Activator.CreateInstance(suitableValidatorType);

            CachedValidators.Add(type, new Tuple<MethodInfo, object>(validateMethodInfo, validatorInstance));

            return new Tuple<MethodInfo, object>(validateMethodInfo, validatorInstance);
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof (object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}