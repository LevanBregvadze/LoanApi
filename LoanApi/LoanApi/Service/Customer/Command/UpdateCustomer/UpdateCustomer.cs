using LoanApi.Data;
using LoanApi.Service.Customer.Query;
using LoanApi.Service.User.Query;

namespace LoanApi.Service.Customer.Command.UpdateCustomer
{
    public class UpdateCustomer : IUpdateCustomer
    {
        private readonly LoanApiContext _context;
        private readonly IGetCustomers _customerService;
        private readonly IGetUser _userService;
        
        public UpdateCustomer(LoanApiContext context, 
            IGetCustomers getService,
            IGetUser userService)

        {
            _context = context;
            _customerService = getService;
            _userService = userService;
           
        }

        public string UpdateCustomerDetailes(Domain.Customer customer, int id, bool isAdmin, int currentUserId)
        {
            

            var requestedCustomer = _customerService.GetCustomerById(id);

            if (requestedCustomer == null)
                return "Customer not found";


            var user = _userService.GetUserById(requestedCustomer.SystemUserId);
            if (user == null)
                return "User not found";


            if (user.ID != currentUserId && !isAdmin)
                return "You don't have access";
           
            if (!isAdmin)
            {
                customer.IsBlock = "True";

            }

            if (customer.IsBlock != "True" &&   customer.IsBlock != "False")
            {
               return  "Status must be either True or False";
            }

            customer.Id = id;
            customer.SystemUserId = requestedCustomer.SystemUserId;
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return "Succesfully updated";
        }
    }
}
