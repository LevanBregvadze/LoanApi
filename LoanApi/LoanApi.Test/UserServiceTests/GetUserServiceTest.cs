using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using LoanApi.Data;
using LoanApi.Domain;
using LoanApi.Service.User.Query;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LoanApi.Test.UserServiceTests
{
    public class GetUserServiceTest
    {
        private readonly DbContextMock<LoanApiContext> dbContextMock;

        public GetUserServiceTest()
        {
            dbContextMock = new DbContextMock<LoanApiContext>(DummyOptions);
            var userDbo = new List<SystemUser>
            {
                new SystemUser { ID = 1, UserName = "LevanBregvadze", FirstName ="Levan", LastName = "Bregvadze", Role ="Admin"},
                new SystemUser { ID = 2, UserName = "LevanBregvadze2"}
            };
            var usersDBSetMock = dbContextMock.CreateDbSetMock(x => x.SystemUsers, userDbo);
        }

        [Test]
        public void GetUserById_ExistingGuidPassed_ResultOK()
        {
            //Arrange
            int id = 1;
            
            var userService = new GetUser(dbContextMock.Object);

            //Act
            var userResult = userService.GetUserById(id);

            //Assert
           Assert.That(userResult.ID, Is.EqualTo(id));
   
        }

        [Test]
        public void GetUserById_UnknownGuidPassed_ResultNotFoundResult()
        {
            //Arrange
            int id = 5;

            var userService = new GetUser(dbContextMock.Object);

            //Act
            var userResult = userService.GetUserById(id);

            //Assert
            Assert.IsNull(userResult);

        }


        [Test]
        public void GetUserById_ShouldReturnAllCustomer_ResultOK()
        {
            //Arrange
            var userList = new List<SystemUser>
            {
                new SystemUser { ID = 1, UserName = "LevanBregvadze", FirstName ="Levan", LastName = "Bregvadze", Role ="Admin"},
                new SystemUser { ID = 2, UserName = "LevanBregvadze2"}
            };

            var userService = new GetUser(dbContextMock.Object);
            

            //Act
            var userResult = userService.GetUserList();

            //Assert
            Assert.That(userResult.Count(), Is.EqualTo(userList.Count()));
            
        }


        [Test]
        public void GetUserById_ShouldReturnAllCustomer_ResultNotEqual()
        {
            //Arrange
            var userList = new List<SystemUser>
            {
                new SystemUser { ID = 1, UserName = "LevanBregvadze", FirstName ="Levan", LastName = "Bregvadze", Role ="Admin"},
            };

            var userService = new GetUser(dbContextMock.Object);

            //Act
            var userResult = userService.GetUserList();

            //Assert
            Assert.That(userResult.Count(), Is.Not.EqualTo(userList.Count()));

        }



        public static DbContextOptions DummyOptions { get; } = new DbContextOptionsBuilder().Options;
    }
}
