using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using LoanApi.Data;
using LoanApi.Domain;
using LoanApi.Service.Customer.Query;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LoanApi.Test.CustomerServiceTest
{
    public class GetCustomersServiceTest
    {
        private readonly DbContextMock<LoanApiContext> dbContextMock;

        public GetCustomersServiceTest()
        {
            dbContextMock = new DbContextMock<LoanApiContext>(DummyOptions);
            var customerDbo = new List<Customer>
            {
                new Customer { Id = 1,  FirstName ="Levan", LastName = "Bregvadze", Age = 25, Salary = 2000, IsBlock = "False", SystemUserId = 2},
                new Customer {  Id = 2,  FirstName ="Levan", LastName = "Bregvadze", Age = 25, Salary = 2000, IsBlock = "False", SystemUserId = 3}
            };
            var customerDBSetMock = dbContextMock.CreateDbSetMock(x => x.Customers, customerDbo);
        }



        [Test]
        public void GetCustomerById_ExistingGuidPassed_ResultOK()
        {
            //Arrange
            int id = 1;
            string firstname = "Levan";
            string lastName = "Bregvadze";

            var customerService = new GetCusttomers(dbContextMock.Object);

            //Act
            var customerResult = customerService.GetCustomerById(id);

            //Assert
            Assert.That(customerResult.Id, Is.EqualTo(id));
            Assert.That(customerResult.FirstName, Is.EqualTo(firstname));
            Assert.That(customerResult.LastName, Is.EqualTo(lastName));

        }


        [Test]
        public void GetCustomerById_UnknownGuidPassed_ResultNotFoundResult()
        {
            //Arrange
            int id = 9;
            
            var customerService = new GetCusttomers(dbContextMock.Object);

            //Act
            var customerResult = customerService.GetCustomerById(id);

            //Assert
            Assert.IsNull(customerResult);
        }


        [Test]
        public void GetAllCustomer_ShouldReturnAllCustomer_ResultOK()
        {
            //Arrange
            var customerList = new List<Customer>
            {
                new Customer { Id = 1,  FirstName ="Levan", LastName = "Bregvadze", Age = 25, Salary = 2000, IsBlock = "False", SystemUserId = 2},
                new Customer {  Id = 2,  FirstName ="Levan", LastName = "Bregvadze", Age = 25, Salary = 2000, IsBlock = "False", SystemUserId = 3}
            };

            var customerService = new GetCusttomers(dbContextMock.Object);

            //Act
            var customerResult = customerService.GetAllCustomer();

            //Assert
            Assert.That(customerResult.Count(), Is.EqualTo(customerList.Count()));
        }


        [Test]
        public void GetAllCustomer_ShouldReturnAllCustomer_ResultNotEqual()
        {
            //Arrange
            var customerList = new List<Customer>
            {
                new Customer { Id = 1,  FirstName ="Levan", LastName = "Bregvadze", Age = 25, Salary = 2000, IsBlock = "False", SystemUserId = 2},
            };

            var customerService = new GetCusttomers(dbContextMock.Object);

            //Act
            var customerResult = customerService.GetAllCustomer();

            //Assert
            Assert.That(customerResult.Count(), Is.Not.EqualTo(customerList.Count()));
        }









        public static DbContextOptions DummyOptions { get; } = new DbContextOptionsBuilder().Options;
    }
}
