using System.Collections.Generic;
using System.Linq;
using LoanApi.Data;
using LoanApi.Domain;

namespace LoanApi.Service.User.Query
{
    public class GetUser : IGetUser
    {

        private readonly LoanApiContext _context;

        public GetUser(LoanApiContext context)
        {
            _context = context;
        }

        public IEnumerable<SystemUser> GetUserList()
        {
            var userList = _context.SystemUsers.ToList();
            return userList;
        }

        public SystemUser GetUserById(int id)
        {
            var user = _context.SystemUsers.Where(x => x.ID == id).FirstOrDefault();

            return user;
        }

        
    }
}
