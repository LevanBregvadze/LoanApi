using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using LoanApi.Data;
using LoanApi.Domain;
using LoanApi.Service.Customer.Command.CreateCustomer;
using LoanApi.Service.Customer.Command.UpdateCustomer;
using LoanApi.Service.Customer.Query;
using LoanApi.Service.User.Query;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace LoanApi.Test.CustomerServiceTest
{
    public class CustomerCommandTest
    {

        private readonly DbContextMock<LoanApiContext> dbContextMock;
        private readonly Mock<IGetCustomers> _customerMock = new Mock<IGetCustomers>();
        private readonly Mock<IGetUser> _userMock = new Mock<IGetUser>();

        public CustomerCommandTest()
        {
            dbContextMock = new DbContextMock<LoanApiContext>(DummyOptions);
            var customerDbo = new List<Customer>
            {
                new Customer {  FirstName ="Levan", LastName = "Bregvadze", Age = 25, Salary = 2000, IsBlock = "True", SystemUserId = 2},
                new Customer {  FirstName ="Levan", LastName = "Bregvadze", Age = 25, Salary = 2000, IsBlock = "True", SystemUserId = 3}
            };
            var customerDBSetMock = dbContextMock.CreateDbSetMock(x => x.Customers, customerDbo);
        }


        [Test]
        public void CreateCustomerRecord_ShouldAddAndReturnCustomer_ReturnOKResult()
        {
            //Arrange
            var newUser = new SystemUser
            {

                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "Admin"

            };
            var expexterIsBlock = "True";
            var customerService = new CreateCustomerCommand(dbContextMock.Object);

            //Act
            var customerResult = customerService.CreateCustomer(newUser);

            //Assert

            Assert.That(newUser.FirstName, Is.EqualTo(customerResult.FirstName));
            Assert.That(newUser.LastName, Is.EqualTo(customerResult.LastName));
            Assert.That(expexterIsBlock, Is.EqualTo(customerResult.IsBlock));
            Assert.That(newUser.ID, Is.EqualTo(customerResult.SystemUserId));

        }


        [Test]
        public void CreateCustomerRecord_ShouldAddAndReturnCustomer_ReturnsIsBlockIsTrue()
        {
            //Arrange
            var newUser = new SystemUser
            {

                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "TestUser",
                Password = "12341234",
                Role = "Admin"

            };



            var expexterIsBlock = "False";
            var customerService = new CreateCustomerCommand(dbContextMock.Object);

            //Act
            var customerResult = customerService.CreateCustomer(newUser);

            //Assert

            Assert.That(newUser.FirstName, Is.EqualTo(customerResult.FirstName));
            Assert.That(newUser.LastName, Is.EqualTo(customerResult.LastName));
            Assert.That(expexterIsBlock, Is.Not.EqualTo(customerResult.IsBlock));
            Assert.That(newUser.ID, Is.EqualTo(customerResult.SystemUserId));

        }


        [Test]
        public void UpdateCustomerDetailes_ShouldReturnUpdatedCustomer_ResultSuccessfully ()
        {
            //Arrange
            var newcustomer = new Customer
            {

                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 2,
                Salary = 4000,

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
            
            string expectetResult = "Succesfully updated";
            var customerService = new UpdateCustomer(dbContextMock.Object, _customerMock.Object, _userMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var customerResult = customerService.UpdateCustomerDetailes(newcustomer, id, isAdmin, currentId);

            //Assert
            Assert.That(customerResult, Is.EqualTo(expectetResult));

        }


        [Test]
        public void UpdateCustomerDetailes_ShouldReturnUpdatedCustomer_ResultCustomerNotFound()
        {
            //Arrange
            var newcustomer = new Customer
            {

                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

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
                Role = "Admin"

            };


            int id = 3;
            int temporaryIncorrectId = 14;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "Customer not found";
            var customerService = new UpdateCustomer(dbContextMock.Object, _customerMock.Object, _userMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(temporaryIncorrectId)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var customerResult = customerService.UpdateCustomerDetailes(newcustomer, id, isAdmin, currentId);

            //Assert
            Assert.That(customerResult, Is.EqualTo(expectetResult));

        }

        [Test]
        public void UpdateCustomerDetailes_ShouldReturnUpdatedCustomer_ResultUserNotFound()
        {
            //Arrange
            var newcustomer = new Customer
            {

                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 1,
                Salary = 4000,

            };

            var existingCustom = new Customer
            {
                Id = 1,
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


            int id = 2;
            int temporaryIncorrectId = 14;
            int currentId = 1;
            bool isAdmin = false;

            string expectetResult = "User not found";
            var customerService = new UpdateCustomer(dbContextMock.Object, _customerMock.Object, _userMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(temporaryIncorrectId)).Returns(user);

            //Act
            var customerResult = customerService.UpdateCustomerDetailes(newcustomer, id, isAdmin, currentId);

            //Assert
            Assert.That(customerResult, Is.EqualTo(expectetResult));

        }


        [Test]
        public void UpdateCustomerDetailes_ShouldReturnUpdatedCustomer_ResultNoAccsses()
        {
            //Arrange
            var newcustomer = new Customer
            {

                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "True",
                SystemUserId = 2,
                Salary = 4000,

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
            var customerService = new UpdateCustomer(dbContextMock.Object, _customerMock.Object, _userMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var customerResult = customerService.UpdateCustomerDetailes(newcustomer, id, isAdmin, currentId);

            //Assert
            Assert.That(customerResult, Is.EqualTo(expectetResult));

        }

        [Test]
        public void UpdateCustomerDetailes_ShouldReturnUpdatedCustomer_ResultStatusTrueOrFalsError()
        {
            //Arrange
            var newcustomer = new Customer
            {

                FirstName = "Levan",
                LastName = "Bregvadze",
                Age = 27,
                IsBlock = "test",
                SystemUserId = 2,
                Salary = 4000,

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
            bool isAdmin = true;

            string expectetResult = "Status must be either True or False";
            var customerService = new UpdateCustomer(dbContextMock.Object, _customerMock.Object, _userMock.Object);
            _customerMock.Setup(x => x.GetCustomerById(id)).Returns(existingCustom);
            _userMock.Setup(x => x.GetUserById(existingCustom.SystemUserId)).Returns(user);

            //Act
            var customerResult = customerService.UpdateCustomerDetailes(newcustomer, id, isAdmin, currentId);

            //Assert
            Assert.That(customerResult, Is.EqualTo(expectetResult));

        }

        public static DbContextOptions DummyOptions { get; } = new DbContextOptionsBuilder().Options;
    }
}
