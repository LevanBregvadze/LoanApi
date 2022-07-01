using FluentValidation;
using LoanApi.Domain;

namespace LoanApi.Service.User.Command.Validator
{
    public class UserValidator : AbstractValidator<SystemUser>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName must be filled")
                .MaximumLength(15).WithMessage("UserName must be less than 15 character");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName must be filled")
                .MaximumLength(15).WithMessage("FirstName must be less than 15 character");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName must be filled")
                .MaximumLength(15).WithMessage("LastName must be less than 15 character");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password must be filled")
                .MinimumLength(8).WithMessage("Password must be greater than 8 char");
        }
    }
}
