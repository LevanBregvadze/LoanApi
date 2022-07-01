using System.Collections.Generic;
using LoanApi.Data;

namespace LoanApi.Service.Loan.Query
{
    public interface ILoanQuery
    {
        
        public IEnumerable<Domain.Loan> GetAllLoan();
        public IEnumerable<Domain.Loan> GetLoanByCustomerId(int id);

    }
}
