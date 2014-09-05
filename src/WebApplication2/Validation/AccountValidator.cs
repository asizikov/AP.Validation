using Domain;
using FluentValidation;

namespace WebApplication2.Validation
{
    internal class AccountValidator : AbstractValidator<Account> 
    {
        public AccountValidator()
        {
            RuleFor(account => account.Name).NotEmpty();
            RuleFor(account => account.Id).GreaterThan(0).WithMessage("id mast be greater then zero");
        }
    }
}
