using System.Collections.Generic;
using System.Linq;
using LoanApi.Data;

namespace LoanApi.Service.Customer.Query
{
    public class GetCusttomers : IGetCustomers
    {
        private readonly LoanApiContext  _context;

        public GetCusttomers(LoanApiContext loanApiContext)
        {
            _context = loanApiContext;
        }
       public IEnumerable<Domain.Customer> GetAllCustomer()
        {
            var customerList = _context.Customers.ToList();
            return customerList;

        }

        public Domain.Customer GetCustomerById(int id)
        {
            var customer = _context.Customers.Where(x => x.Id == id).FirstOrDefault();
            return customer;

        }

       

    }
}
