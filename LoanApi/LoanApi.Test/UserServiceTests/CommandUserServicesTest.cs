using System.Collections.Generic;
using EntityFrameworkCoreMock;
using LoanApi.Data;
using LoanApi.Domain;
using LoanApi.Service.User.Command.CreateUser;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace LoanApi.Test.UserServiceTests
{
    public class CommandUserServicesTest
    {
        private readonly DbContextMock<LoanApiContext> dbContextMock;

        public CommandUserServicesTest()
        {
            dbContextMock = new DbContextMock<LoanApiContext>(DummyOptions);
            var userDbo = new List<SystemUser>
            {
               new SystemUser {  UserName = "LevanBregvadze"},
               new SystemUser {  UserName = "LevanBregvadze2"}
            };
            var usersDBSetMock = dbContextMock.CreateDbSetMock(x => x.SystemUsers, userDbo);
        }


        [Test]
        public void CreateUserRecord_ShouldAddUserAndReturnUser_ReturnOKResult()
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

           
            var userService = new CreateUserCommand(dbContextMock.Object);

            //Act
            var userResult = userService.CreateUserRecord(newUser);

            //Assert
            Assert.IsNotEmpty(userResult.UserName);
            Assert.IsNotEmpty(userResult.Password);
            Assert.IsNotEmpty(userResult.Role);
            Assert.IsNotEmpty(userResult.FirstName);
            Assert.IsNotEmpty(userResult.LastName);
            dbContextMock.Verify(x => x.Add(newUser), Times.Once);

        }



        [Test]
        public void CreateUserRecord_ShouldAddUserAndReturnUser_ReturnNull()
        {
            //Arrange
            var newUser = new SystemUser
            {
                
                FirstName = "Levan",
                LastName = "Bregvadze",
                UserName = "LevanBregvadze2",
                Password = "12341234",
                Role = "Admin"

            };

            var userService = new CreateUserCommand(dbContextMock.Object);

            //Act
            var userResult = userService.CreateUserRecord(newUser);

            //Assert
            Assert.IsNull(userResult);

        }




        public static DbContextOptions DummyOptions { get; } = new DbContextOptionsBuilder().Options;
    }
}