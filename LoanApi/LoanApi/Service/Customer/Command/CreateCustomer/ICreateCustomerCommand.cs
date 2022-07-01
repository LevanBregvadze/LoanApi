using LoanApi.Domain;

namespace LoanApi.Service.Customer.Command.CreateCustomer
{
    public interface ICreateCustomerCommand
    {

        public LoanApi.Domain.Customer CreateCustomer(SystemUser systemUser);
    }
}
