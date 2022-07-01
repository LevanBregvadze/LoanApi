using FluentValidation;

namespace LoanApi.Service.Loan.Validator
{
    public class LoanValidator : AbstractValidator<Domain.Loan>
    {
        public LoanValidator()
        {
            RuleFor(x => x.LoanType).NotEmpty().WithMessage("LoanType must be filled");
            RuleFor(x => x.LoanPeriod).NotEmpty().WithMessage("LoanPeriod must be filled");
            RuleFor(x => x.Currency).NotEmpty().WithMessage("Currency must be filled");
            RuleFor(x => x.Ammount).NotEmpty().WithMessage("Ammount must be filled");
        }
    }
}
