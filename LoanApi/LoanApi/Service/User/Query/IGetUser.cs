using System.Collections;
using System.Collections.Generic;
using LoanApi.Domain;

namespace LoanApi.Service.User.Query
{
    public interface IGetUser 
    {

        public IEnumerable<SystemUser> GetUserList();

        public SystemUser GetUserById(int id);
    }
}
