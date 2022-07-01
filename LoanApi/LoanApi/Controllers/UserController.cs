using System;
using System.Collections.Generic;
using LoanApi.Domain;
using LoanApi.Service.Common.Login;
using LoanApi.Service.Common.Token;
using LoanApi.Service.Customer.Command.CreateCustomer;
using LoanApi.Service.User.Command.CreateUser;
using LoanApi.Service.User.Command.Validator;
using LoanApi.Service.User.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoanApi.Controllers
{
    [Authorize]
    [Route("LoanApi/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserLoginCommand _loginService;
        private readonly IGenerateTokenCommand _tokenSerrvice;
        private readonly ICreateUserCommand _userService;
        private readonly ICreateCustomerCommand _customerService;
        private readonly IGetUser _getUser;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserLoginCommand loginService, 
            IGenerateTokenCommand tokenSerrvice, 
            ICreateUserCommand userService, 
            ICreateCustomerCommand customerService,
            IGetUser getUser,
            ILogger<UserController> logger)
        {
            _loginService = loginService;
            _tokenSerrvice = tokenSerrvice;
            _userService = userService;
            _customerService = customerService;
            _getUser = getUser;
            _logger = logger;
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Registration([FromBody] SystemUser sysUser)
        {
            try
            {
                var validator = new UserValidator();
                var result = validator.Validate(sysUser);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors[0].ErrorMessage);
                }

                var user = _userService.CreateUserRecord(sysUser);
                if (user == null)
                {
                    return BadRequest(new { message = "Username already exist" });
                }

                if (sysUser.Role == "User")
                {
                    _customerService.CreateCustomer(sysUser);
                }

                return Ok(sysUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }


        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            try
            {
                var user = _loginService.Login(userLogin);
                if (user == null)
                    return BadRequest(new { message = "Username or Password is incorrect" });
                string tokenString = _tokenSerrvice.GenerateToken(user);

                return Ok(new
                {
                    Id = user.ID,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    Token = tokenString
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }



        [Authorize(Roles = Role.Admin)]
        [Authorize]
        [HttpGet("Users")]
        public ActionResult<IEnumerable<SystemUser>> getAllUser()
        {
            try
            {
                var users = _getUser.GetUserList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }


        }


        [Authorize]
        [HttpGet("User{id}")]
        public ActionResult getUserById(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.Identity.Name);
                if (id != currentUserId && !User.IsInRole(Role.Admin))
                    return Forbid();

                var user = _getUser.GetUserById(id);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

    }
}
