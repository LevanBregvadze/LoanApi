using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using LoanApi.Data;
using LoanApi.Domain;
using LoanApi.Service.Common.Login;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LoanApi.Test.CommonServiceTest
{
    public class UserLoginServiceTest
    {

        private readonly DbContextMock<LoanApiContext> dbContextMock;
        
        public UserLoginServiceTest()
        {
            var pass = BCrypt.Net.BCrypt.HashPassword("12341234");

            dbContextMock = new DbContextMock<LoanApiContext>(DummyOptions);
            var userDbo = new List<SystemUser>
            {
               new SystemUser {  UserName = "LevanBregvadze", Password = pass},
               new SystemUser {  UserName = "LevanBregvadze2", Password = pass}
            };
            var loginDBSetMock = dbContextMock.CreateDbSetMock(x => x.SystemUsers, userDbo);
        }


        [Test]
        public void Login_ShouldReturnUser_ResultOK()
        {
            //Arrange
            int id = 1;
            

            var user = new UserLogin
            {
                UserName = "LevanBregvadze",
                Password = "12341234",

            };

            var loginService = new UserLoginCommand(dbContextMock.Object);

            //Act
            var userResult = loginService.Login(user);

            //Assert
            Assert.That(userResult.ID, Is.EqualTo(id));
        }


        [Test]
        public void Login_ShouldReturnUser_ResultNull()
        {
            //Arrange
            int id = 1;


            var user = new UserLogin
            {
                UserName = "LevanBregvadze",
                Password = "123411111",

            };

            var loginService = new UserLoginCommand(dbContextMock.Object);

            //Act
            var userResult = loginService.Login(user);

            //Assert
            Assert.IsNull(userResult);

        }


        public static DbContextOptions DummyOptions { get; } = new DbContextOptionsBuilder().Options;
    }
}
