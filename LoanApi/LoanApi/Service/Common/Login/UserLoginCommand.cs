using System.Linq;
using LoanApi.Data;
using LoanApi.Domain;

namespace LoanApi.Service.Common.Login
{
    public class UserLoginCommand : IUserLoginCommand
    {
        private readonly LoanApiContext _context;

        public UserLoginCommand(LoanApiContext context)
        {
            _context = context;
        }

        public SystemUser Login(UserLogin loginModel)
        {

            if (string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
                return null;

            var user = _context.SystemUsers.SingleOrDefault(x => x.UserName == loginModel.UserName);

            if (user == null)
                return null;

            bool IsValidPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password);

            if (!IsValidPassword)
                return null;

            return user;
        }
    }
}
