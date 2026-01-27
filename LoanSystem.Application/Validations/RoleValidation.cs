using FluentValidation;
using LoanSystem.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanSystem.Application.Validations
{
    public class RoleValidation : AbstractValidator<RegisterDto>
    {
        public RoleValidation()
        {
            RuleFor(u => u.Role)
                .Matches("^(Customer|Admin)$")
                .WithMessage("The Role must be ether Admin Or Customer");
        }
    }
}
