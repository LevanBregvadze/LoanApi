using System.Collections.Generic;

namespace LoanApi.Service.Customer.Query
{
    public interface IGetCustomers
    {
        


        public IEnumerable<LoanApi.Domain.Customer> GetAllCustomer();
        public LoanApi.Domain.Customer GetCustomerById(int id);

    }
}
