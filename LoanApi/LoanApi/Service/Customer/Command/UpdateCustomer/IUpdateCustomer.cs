namespace LoanApi.Service.Customer.Command.UpdateCustomer
{
    public interface IUpdateCustomer
    {
        public string UpdateCustomerDetailes(LoanApi.Domain.Customer customer, int id, bool isAdmin, int currentUserId);
    }
}
