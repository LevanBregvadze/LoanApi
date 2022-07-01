using System;
using System.Collections.Generic;
using LoanApi.Domain;
using LoanApi.Service.Customer.Query;
using LoanApi.Service.Loan.Command;
using LoanApi.Service.Loan.Query;
using LoanApi.Service.Loan.Validator;
using LoanApi.Service.User.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoanApi.Controllers
{
    [Authorize]
    [Route("LoanApi/[controller]")]
    [ApiController]
    public class LoanController : Controller
    {
        private readonly ILoanCommand _commandService;
        private readonly ILoanQuery _queryService;
        private readonly IGetCustomers _customerService;
        private readonly IGetUser _userService;
        private readonly ILogger<LoanController> _logger;

        public LoanController(ILoanCommand commandService, 
            IGetCustomers customerService, 
            IGetUser userService,
            ILoanQuery queryService,
             ILogger<LoanController> logger)
        {
            _commandService = commandService;
            _queryService = queryService;
            _customerService = customerService;
            _userService = userService;
            _logger = logger;   

        }

        [Authorize]
        [HttpPost("Add{customerId}")]
        public IActionResult RequestLoanByCustomerId(Loan loan, int customerId)
        {
            try
            {
                var validator = new LoanValidator();
                var result = validator.Validate(loan);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors[0].ErrorMessage);
                }

                var currentUserId = int.Parse(User.Identity.Name);
                var isAdmin = User.IsInRole("Admin");
                var requestedLoan = _commandService.AddLoan(loan, customerId, isAdmin, currentUserId);
                return Ok(requestedLoan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }



        }



        [Authorize(Roles = Role.Admin)]
        [Authorize]
        [HttpGet("Loans")]
        public ActionResult<IEnumerable<Loan>> GetAllLoan()
        {
            try
            {
                var loans = _queryService.GetAllLoan(); ;
                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }



        [Authorize]
        [HttpGet("Loan{customerId}")]
        public IActionResult GetLoanByCustomerId(int customerId)
        {
            try
            {
                var currentUserId = int.Parse(User.Identity.Name);
                var customer = _customerService.GetCustomerById(customerId);

                if (customer == null)
                    return NotFound();

                var user = _userService.GetUserById(customer.SystemUserId);

                if (user.ID != currentUserId && !User.IsInRole(Role.Admin))
                    return Forbid();

                var loan = _queryService.GetLoanByCustomerId(customerId);

                if (loan == null)
                    return NotFound();
                return Ok(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }


        }

        [Authorize]
        [HttpPut("Remove{id}")]
        public IActionResult DeleteLoan(int id)
        {

            try
            {
                var currentUserId = int.Parse(User.Identity.Name);
                var isAdmin = User.IsInRole("Admin");


                var deleteLoan = _commandService.DeleteLoan(id, currentUserId, isAdmin);
                return Ok(deleteLoan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }


        [Authorize]
        [HttpPut("Update{id}")]
        public IActionResult UpdateLoan(Loan toBeUpdated, int id)
        {
           

            try
            {
                var validator = new LoanValidator();
                var result = validator.Validate(toBeUpdated);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors[0].ErrorMessage);
                }
                var currentUserId = int.Parse(User.Identity.Name);
                var isAdmin = User.IsInRole("Admin");


                var updateLoan = _commandService.UpdateLoan(toBeUpdated, id, currentUserId, isAdmin);
                return Ok(updateLoan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
    }
}
