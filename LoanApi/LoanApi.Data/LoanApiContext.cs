using LoanApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace LoanApi.Data
{
	public class LoanApiContext : DbContext
	{
		public LoanApiContext(DbContextOptions options) :
			base(options)
		{
			Options = options;
		}

		//public LoanApiContext(DbContextOptions<LoanApiContext> options) :
		//	base(options)
		//{
		//	Options = options;
		//}

		public DbContextOptions Options { get; }
		public virtual DbSet<SystemUser> SystemUsers { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }

		public virtual DbSet<Loan> Loans { get; set; }



	}
}
