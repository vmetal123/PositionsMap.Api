using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Contracts.V1.Requests.Validators
{
    public class RoleCreationRequestValidator: AbstractValidator<RoleCreationRequest>
    {
        public RoleCreationRequestValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty()
                .NotNull();
        }
    }
}
