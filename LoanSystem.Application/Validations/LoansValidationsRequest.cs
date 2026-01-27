using FluentValidation;
using LoanSystem.Application.Requests;

namespace LoanSystem.Application.Validations
{
    public class LoansValidationsRequest : AbstractValidator<LoanRequest>
    {
        public LoansValidationsRequest()
        {
            RuleFor(l => l.BookId)
                .GreaterThan(0)
                .WithMessage("The BookId must be greater than 0");
        }
    }
}
