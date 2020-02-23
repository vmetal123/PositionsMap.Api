using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Contracts.V1.Requests.Validators
{
    public class UserLoginRequestValidator: AbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty()
                .NotNull();
            RuleFor(v => v.Password)
                .NotEmpty()
                .NotNull();
        }
    }
}
