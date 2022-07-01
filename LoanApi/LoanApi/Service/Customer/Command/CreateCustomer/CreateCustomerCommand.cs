using LoanApi.Data;
using LoanApi.Domain;
using LoanApi.Service.Customer.Command.UpdateCustomer;
using LoanApi.Service.User.Query;

namespace LoanApi.Service.Customer.Command.CreateCustomer
{
    public class CreateCustomerCommand : ICreateCustomerCommand
    {
        private readonly LoanApiContext _context;
        
        public CreateCustomerCommand(LoanApiContext context)
        {
            _context = context;
          

        }

        public Domain.Customer CreateCustomer(SystemUser systemUser)
        {
            Domain.Customer  customer = new Domain.Customer
            {
                FirstName = systemUser.FirstName,
                LastName = systemUser.LastName,
                IsBlock = "True",
                SystemUserId = systemUser.ID
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer;
        }





    }
}
