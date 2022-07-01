using System.Collections.Generic;
using System.Linq;
using LoanApi.Data;

namespace LoanApi.Service.Loan.Query
{
    public class LoanQuery : ILoanQuery
    {
        private readonly LoanApiContext _context;

        public LoanQuery(LoanApiContext context)
        {
            _context = context;
        }

        public IEnumerable<Domain.Loan> GetAllLoan()
        {
            var loans = _context.Loans.ToList();
            return loans;
        }

        public IEnumerable<Domain.Loan> GetLoanByCustomerId(int id)
        {
            var loan = _context.Loans.Where(x => x.CustomerId == id).ToList();
            return loan;

        }
    }
}
