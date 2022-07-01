using System;
using System.Linq;
using LoanApi.Data;
using LoanApi.Domain;
using Microsoft.Extensions.Logging;

namespace LoanApi.Service.User.Command.CreateUser
{
    public class CreateUserCommand : ICreateUserCommand
    {
        private readonly LoanApiContext _context;
        

        public CreateUserCommand(LoanApiContext context)
        {
            _context = context;
            
        }



        public SystemUser CreateUserRecord(SystemUser sysUser)
        {

            var user = _context.SystemUsers.SingleOrDefault(x => x.UserName == sysUser.UserName);


            if (user == null)
            {
                sysUser.Password = BCrypt.Net.BCrypt.HashPassword(sysUser.Password);
                _context.Add(sysUser);
                _context.SaveChanges();
                return sysUser;
            }

            
            return null;

        }
           


        

    }
}
