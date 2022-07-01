using System;
using System.Linq;
using LoanApi.Data;
using LoanApi.Service.Customer.Query;
using LoanApi.Service.User.Query;

namespace LoanApi.Service.Loan.Command
{
    public class LoanCommand : ILoanCommand
    {
        private readonly LoanApiContext _context;
        private readonly IGetUser _userService;
        private readonly IGetCustomers _customerService;

        public LoanCommand(LoanApiContext context, 
            IGetUser userService,
            IGetCustomers customerService)
        {
            _context = context;
            _userService = userService;
            _customerService = customerService;
        }

        public string AddLoan(Domain.Loan loan, int id, bool isAdmin, int currentUserId)
        {
            var requestedCustomer = _customerService.GetCustomerById(id);
            if (requestedCustomer == null)
                return "Customer Not Found";

            var user = _userService.GetUserById(requestedCustomer.SystemUserId);
            if (user == null)
            {
                return "User not found";
            }

            if (user.ID != currentUserId && !isAdmin)
                return "You don't have access";

            var isNotBlock = requestedCustomer.IsBlock == "True";

            if (!isAdmin && !isNotBlock)
            {
                return  "You are blocked";
            }

            var isValidData = requestedCustomer.Salary != 0 && requestedCustomer.Age > 18;
            if (!isValidData)
            {
                return  "Customer data does not meet the requirements";
            }

            loan.Status = "In Progress";
            loan.CustomerId = requestedCustomer.Id;
            _context.Loans.Add(loan);
            _context.SaveChanges();
            
            return "Loan successfuly requested";

        }

        public string DeleteLoan( int id, int currentUserId, bool isAdmin)
        {
            var loan = _context.Loans.Where(x => x.Id == id).SingleOrDefault();
            if (loan  == null)
                return "Not Found";

            var validator = ValidateAccess(loan.CustomerId, currentUserId, isAdmin);
            if (validator != null)
            {
                return validator;
            }

            var isInProgress = loan.Status == "In Progress";

            if (!isInProgress)
            {
                return "To delete the status must be In Progress";
            }

            _context.Loans.Remove(loan);
            _context.SaveChanges();
            return "Successfully Removed";
        }

        public string UpdateLoan(Domain.Loan tobeUpdated, int id, int currentUserId, bool isAdmin)
        {
            var loan = _context.Loans.Where(x => x.Id == id).SingleOrDefault();
            if (loan == null)
                return "Not Found";

            var validator = ValidateAccess(loan.CustomerId, currentUserId, isAdmin);
            if (validator != null)
            {
                return validator;
            }

            var isInProgress = loan.Status == "In Progress";

            if (!isInProgress && !isAdmin)
            {
                return "To Update the status must be In Progress";
            }

            if (!isAdmin)
            {
                tobeUpdated.Status = "In Progress";
            }

            tobeUpdated.Id = id;
            tobeUpdated.CustomerId = loan.CustomerId;
            _context.Loans.Update(tobeUpdated);
            _context.SaveChanges();
            return "Updated Successfuly";
        }


        private string ValidateAccess(int id, int currentUserId, bool isAdmin)
        {
            var requestedCustomer = _customerService.GetCustomerById(id);
            if (requestedCustomer == null)
                return "Customer Not Found";

            var user = _userService.GetUserById(requestedCustomer.SystemUserId);
            if (user == null)
            {
                return "User not found";
            }

            if (user.ID != currentUserId && !isAdmin)
                return "You don't have access";

            return null;
        }
    }
}