using LoanApi.Domain;

namespace LoanApi.Service.User.Command.CreateUser
{
    public interface ICreateUserCommand
    {

        public SystemUser CreateUserRecord(SystemUser sysUser);

        
    }
}
