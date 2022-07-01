namespace LoanApi.Service.Loan.Command
{
    public interface ILoanCommand
    {

        public string AddLoan(Domain.Loan loan, int id, bool isAdmin, int currentCustomerId);

        public string DeleteLoan( int id, int currentUserId, bool isAdmin);

        public string UpdateLoan(Domain.Loan tobeUpdated, int id, int currentUserId, bool isAdmin);
    }
}
