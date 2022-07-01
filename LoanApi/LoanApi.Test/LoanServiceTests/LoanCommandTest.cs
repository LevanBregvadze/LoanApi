using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using LoanApi.Data;
using LoanApi.Domain;
using LoanApi.Service.Customer.Query;
using LoanApi.Service.Loan.Command;
using LoanApi.Service.User.Query;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace LoanApi.Test.LoanServiceTests
{
    public class LoanCommandTest
    {

        private DbContextMock<LoanApiContext> dbContextMock;
        private readonly Mock<IGetCustomers> _customerMock = new Mock<IGetCustomers>();
        private readonly Mock<IGetUser> _userMock = new Mock<IGetUser>();


        public LoanCommandTest()
        {
            dbContextMock = new DbContextMock<LoanApiContext>(DummyOptions);
            var loanDbo = new List<Loan>
            {
                new Loan { Id = 1, Ammount = 2000, Currency = "Gel", CustomerId = 1, LoanPeriod = 12, LoanType = "Test", Status = "In Progress" },
                new Loan { Id = 2, Ammount = 2000, Currency = "Gel", CustomerId = 2, LoanPeriod = 12, LoanType = "Test", Status = "In Progress" },
                new Loan { Id = 30, Ammount = 2000, Currency = "Gel", CustomerId = 2, LoanPeriod = 12, LoanType = "Test", Status = "Approved"},
                new Loan { Id = 31, Ammount = 2000, Currency = "Gel", CustomerId = 1, LoanPeriod = 12, LoanType = "Test", Status = "In Progress" },
                new Loan { Id = 32, Ammount = 2000, Currency = "Gel", CustomerId = 1, LoanPeriod = 12, LoanType = "Test", Status = "In Progress" },
                new Loan { Id = 33, Ammount = 2000, Currency = "Gel", CustomerId = 5, LoanPeriod = 12, LoanType = "Test", Status = "In Progress" },
                new Loan { Id = 34, Ammount = 2000, Currency = "Gel", CustomerId = 1, LoanPeriod = 12, LoanType = "Test", Status = "Approved" },
            };
            var loanDBSetMock = dbContextMock.CreateDbSetMock(x => x.Loans, loanDbo);
        }



        [Test]
        public void AddLoan_ShouldAddLoan_ResultSuccessfull()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 3,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "Admin"

            };


            int id = 1;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "Loan successfuly requested";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.AddLoan(newLoan, id, isAdmin, currentId);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));
            

        }

        [Test]
        public void AddLoan_ShouldAddLoan_ResultCustomerNotFound()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 4,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 2,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 2;
            int currentId = 1;
            int temporaryIncorrectId = 14;
            bool isAdmin = false;

            string expectetResult = "Customer Not Found";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(temporaryIncorrectId)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.AddLoan(newLoan, id, isAdmin, currentId);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));
        }

        [Test]
        public void AddLoan_ShouldAddLoan_ResultUserFound()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 10,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 2,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 2,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "Admin"

            };


            int id = 1;
            int currentId = 1;
            int temporaryIncorrectId = 14;
            bool isAdmin = false;

            string expectetResult = "User not found";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(temporaryIncorrectId)).Returns(user);

            //Act
            var loanResult = loanService.AddLoan(newLoan, id, isAdmin, currentId);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));
        }

        [Test]
        public void AddLoan_ShouldAddLoan_ResultNotAccess()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 5,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 2,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "Admin"

            };


            int id = 1;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "You don't have access";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.AddLoan(newLoan, id, isAdmin, currentId);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }


        [Test]
        public void AddLoan_ShouldAddLoan_ResultIsBlock()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 6,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "False",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 1;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "You are blocked";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.AddLoan(newLoan, id, isAdmin, currentId);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }


        [Test]
        public void AddLoan_ShouldAdd_ResultInvalidData()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 3,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 12,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "Admin"

            };


            int id = 1;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "Customer data does not meet the requirements";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.AddLoan(newLoan, id, isAdmin, currentId);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }



        [Test]
        public void UpdateLoan_ShouldUpdate_ResultSuccessfull()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 2,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 2,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 1;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "Updated Successfuly";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.UpdateLoan(newLoan, id, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }


        [Test]
        public void UpdateLoan_ShouldUpdate_ResultCustomerNotFound()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 4,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 2,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 2;
            int currentId = 1;
            int temporaryIncorrectId = 14;
            bool isAdmin = false;

            string expectetResult = "Customer Not Found";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(temporaryIncorrectId)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.UpdateLoan(newLoan, id, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }



        [Test]
        public void UpdateLoan_ShouldUpdate_ResultUserNotFound()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 4,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 2,

            };



            var existingCustom = new Customer
            {
                Id = 2,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 2,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 2;
            int currentId = 1;
            int temporaryIncorrectId = 14;
            bool isAdmin = false;

            string expectetResult = "User not found";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(temporaryIncorrectId)).Returns(user);

            //Act
            var loanResult = loanService.UpdateLoan(newLoan, id, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }


        [Test]
        public void UpdateLoan_ShouldUpdate_ResultLoanNotFound()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 4,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 2,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 2;
            int currentId = 1;
            int temporaryIncorrectId = 14;
            bool isAdmin = false;

            string expectetResult = "Not Found";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.UpdateLoan(newLoan, temporaryIncorrectId, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }


        [Test]
        public void UpdateLoan_ShouldUpdate_ResultMustBeInProgress()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 2,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 1,

            };



            var existingCustom = new Customer
            {
                Id = 2,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 30;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "To Update the status must be In Progress";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.UpdateLoan(newLoan, id, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }


        [Test]
        public void UpdateLoan_ShouldUpdate_ResultNoAcces()
        {
            //Arrange
            var newLoan = new Loan
            {
                Id = 4,
                Ammount = 2000,
                Status = "In Progress",
                Currency = "Gel",
                LoanPeriod = 12,
                LoanType = "Test",
                CustomerId = 2,

            };



            var existingCustom = new Customer
            {
                Id = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 3,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 2;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "You don't have access";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.UpdateLoan(newLoan, id, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));

        }


        [Test]
        public void DeleteLoan_ShouldDelete_ResultSuccessfull()
        {
            //Arrange


            var existingCustom = new Customer
            {
                Id = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 1;
            int loanId = 31;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "Successfully Removed";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.DeleteLoan(loanId, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }

        [Test]
        public void DeleteLoan_ShouldDelete_ResultNoAccess()
        {
            //Arrange


            var existingCustom = new Customer
            {
                Id = 2,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 21,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 23,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 1;
            int loanId = 32;
            int currentId = 2;
            bool isAdmin = false;

            string expectetResult = "You don't have access";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.DeleteLoan(loanId, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }

        [Test]
        public void DeleteLoan_ShouldDelete_ResultCustomerNotFound()
        {
            //Arrange


            var existingCustom = new Customer
            {
                Id = 4,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 4,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 4;
            int loanId = 33;
            int temporaryId = 3;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "Customer Not Found";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(temporaryId)).Returns(user);

            //Act
            var loanResult = loanService.DeleteLoan(loanId, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));


        }


        [Test]
        public void DeleteLoan_ShouldDelete_ResultStatusMustBeInProgress()
        {
            //Arrange


            var existingCustom = new Customer
            {
                Id = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 1;
            int loanId = 34;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "To delete the status must be In Progress";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.DeleteLoan(loanId, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));




        }

        [Test]
        public void DeleteLoan_ShouldDelete_ResultNotFound()
        {
            //Arrange


            var existingCustom = new Customer
            {
                Id = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var user = new SystemUser
            {
                ID = 1,
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "User"

            };


            int id = 1;
            int loanId = 35;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "Not Found";

            var loanService = new LoanCommand(dbContextMock.Object, _userMock.Object, _customerMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var loanResult = loanService.DeleteLoan(loanId, currentId, isAdmin);


            //Assert
            Assert.That(loanResult, Is.EqualTo(expectetResult));

        }


        public static DbContextOptions DummyOptions { get; } = new DbContextOptionsBuilder().Options;
    }
}
