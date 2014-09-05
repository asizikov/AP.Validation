using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using FluentValidation;

namespace Validation.ValidationRules
{
    internal class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(account => account.Name).NotEmpty();
            RuleFor(account => account.Id).GreaterThan(0).WithMessage("id can't be zero");
        }
    }
}
