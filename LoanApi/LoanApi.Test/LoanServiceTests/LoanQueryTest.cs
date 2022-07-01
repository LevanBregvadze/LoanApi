using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using LoanApi.Data;
using LoanApi.Domain;
using LoanApi.Service.Loan.Query;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LoanApi.Test.LoanServiceTests
{
    public class LoanQueryTest
    {
        private DbContextMock<LoanApiContext> dbContextMock;

        public LoanQueryTest()
        {
            dbContextMock = new DbContextMock<LoanApiContext>(DummyOptions);
            var loanDbo = new List<Loan>
            {
                new Loan { Id = 1, Ammount = 2000, Currency = "Gel", CustomerId = 1, LoanPeriod = 12, LoanType = "Test", Status = "In progress" },
                new Loan { Id = 2, Ammount = 2000, Currency = "Gel", CustomerId = 2, LoanPeriod = 12, LoanType = "Test", Status = "Approved"}
            };
            var loanDBSetMock = dbContextMock.CreateDbSetMock(x => x.Loans, loanDbo);
        }



        [Test]
        public void GetLoanByCustomerId_ExistingGuidPassed_ResultOK()
        {
            //Arrange
            int id = 1;
            int numberOfLoans = 1;

            var loanService = new LoanQuery(dbContextMock.Object);

            //Act
            var loanResult = loanService.GetLoanByCustomerId(id);

            //Assert
            Assert.That(numberOfLoans, Is.EqualTo(loanResult.Count()));
            Assert.That(loanResult.FirstOrDefault().Id, Is.EqualTo(id));

        }


        [Test]
        public void GetLoanByCustomerId_UnknownGuidPassed_ResultNotFoundResul()
        {
            //Arrange
            int id = 3;

            var loanService = new LoanQuery(dbContextMock.Object);

            //Act
            var loanResult = loanService.GetLoanByCustomerId(id);

            //Assert

            Assert.IsEmpty(loanResult);
        }


        [Test]
        public void GetAllLoan_ShouldReturnAllLoan_ResultOK()
        {
            //Arrange
            
            int numberOfLoans = 2;

            var loanService = new LoanQuery(dbContextMock.Object);

            //Act
            var loanResult = loanService.GetAllLoan();

            //Assert
            Assert.That(numberOfLoans, Is.EqualTo(loanResult.Count()));
            
        }


        [Test]
        public void GetAllLoan_ShouldReturnAllLoan_ResultNotEqual()
        {
            //Arrange

            int numberOfLoans = 1;

            var loanService = new LoanQuery(dbContextMock.Object);

            //Act
            var loanResult = loanService.GetAllLoan();

            //Assert
            Assert.That(numberOfLoans, Is.Not.EqualTo(loanResult.Count()));

        }



        public static DbContextOptions DummyOptions { get; } = new DbContextOptionsBuilder().Options;
    }
}
