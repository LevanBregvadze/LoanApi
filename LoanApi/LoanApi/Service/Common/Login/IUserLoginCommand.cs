using LoanApi.Domain;

namespace LoanApi.Service.Common.Login
{
    public interface IUserLoginCommand
    {
        SystemUser Login(UserLogin loginModel);
    }
}
