using System;
using System.Collections.Generic;
using LoanApi.Domain;
using LoanApi.Service.Customer.Command.UpdateCustomer;
using LoanApi.Service.Customer.Query;
using LoanApi.Service.User.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoanApi.Controllers
{
    [Authorize]
    [Route("LoanApi/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly IGetCustomers _customerService;
        private readonly IGetUser _userService;
        private readonly IUpdateCustomer _updateService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(
            IGetCustomers getService, 
            IGetUser userService, 
            IUpdateCustomer updateService,
            ILogger<CustomerController> logger)
        {
            _customerService = getService;
            _userService = userService;
            _updateService = updateService;
            _logger = logger;   
        }



        [Authorize(Roles = Role.Admin)]
        [Authorize]
        [HttpGet("Customers")]
        public ActionResult<IEnumerable<Customer>> GetAllCustomer()
        {
            try
            {
                var customers = _customerService.GetAllCustomer();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
            
        }



        [Authorize]
        [HttpGet("Customer{id}")]
        public IActionResult GetCustomerById(int id)
        {
            
            try
            {
                var currentUserId = int.Parse(User.Identity.Name);
                var customer = _customerService.GetCustomerById(id);

                if (customer == null)
                    return NotFound();

                var user = _userService.GetUserById(customer.SystemUserId);

                if (user.ID != currentUserId && !User.IsInRole(Role.Admin))
                    return Forbid();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }


        }

        [Authorize]
        [HttpPut("Update{id}")]
        public ActionResult<Customer> UpdateById(Customer customer, int id)
        {
            try
            {
                var currentUserId = int.Parse(User.Identity.Name);
                var isAdmin = User.IsInRole("Admin");
                var response = _updateService.UpdateCustomerDetailes(customer, id, isAdmin, currentUserId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }



    }
}
