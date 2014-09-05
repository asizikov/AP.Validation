using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Validation.ValidationRules;

namespace Validation
{
    class Program
    {
        static void Main(string[] args)
        {
            var account = new Account { Id = 0, Name = null };
            var validator = new AccountValidator();
            validator.Validate(account);
        }
    }

    public static class ValidationExt
    {
        public static void Validate(this object target)
        {
            var type = target.GetType();

        }
    }
}
