using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Contracts.V1.Requests.Validators
{
    public class UserRegistrationRequestValidator: AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty()
                .NotNull();
            RuleFor(v => v.Password)
                .NotEmpty()
                .NotNull();
            RuleFor(v => v.FirstName)
                .NotEmpty()
                .NotNull();
            RuleFor(v => v.LastName)
                .NotEmpty()
                .NotNull();
        }
    }
}
