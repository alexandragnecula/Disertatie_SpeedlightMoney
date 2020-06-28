using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Loans;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LoanController : ControllerBase
    {
        ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost("addloan")]
        public async Task<IActionResult> AddLoan([FromBody] LoanDto loanToAdd)
        {
            if (loanToAdd.Description == null)
            {
                return BadRequest("The loan description is mandatory!");
            }
            var result = await _loanService.AddLoan(loanToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UpdateLoan([FromBody] LoanDto loanToUpdate)
        {
            if (loanToUpdate.Description == null)
            {
                return BadRequest("The loan description is mandatory!");
            }

            var result = await _loanService.UpdateLoan(loanToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{loanId}")]
        public async Task<ActionResult<Result>> DeleteCurrency(long loanId)
        {
            var result = await _loanService.DeleteLoan(new LoanDto { Id = loanId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("restore")]
        public async Task<ActionResult<Result>> RestoreCurrency(long loanId)
        {
            var result = await _loanService.RestoreLoan(new LoanDto { Id = loanId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetLoanById(long id)
        {
            var vm = await _loanService.GetLoanById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid loan was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<LoanDto>> GetAllLoans()
        {
            var vm = await _loanService.GetAllLoans();

            return Ok(vm);
        }

        [HttpGet("loansdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetLoansDropdown()
        {
            var vm = await _loanService.GetAllAsSelect(new LoanDto());

            return Ok(vm);
        }

        [HttpPost("requestloan")]
        public async Task<IActionResult> RequestLoan([FromBody] LoanDto loanToAdd)
        {
            if (loanToAdd.Description == null)
            {
                return BadRequest("The loan description is mandatory!");
            }
            var result = await _loanService.RequestLoan(loanToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("manageloan")]
        public async Task<ActionResult<Result>> ApproveLoan([FromBody] LoanDto loanToUpdate)
        {
            var result = await _loanService.ManageLoan(loanToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("borrowrequests")]
        public async Task<ActionResult<IList<LoanDto>>> GetBorrowerRequestsForCurrentUser()
        {
            var vm = await _loanService.GetBorrowRequestsForCurrentUser();

            return Ok(vm);
        }

        [HttpGet("lendrequests")]
        public async Task<ActionResult<IList<LoanDto>>> GetLenderRequestsForCurrentUser()
        {
            var vm = await _loanService.GetLendRequestsForCurrentUser();

            return Ok(vm);
        }

        [HttpGet("borrowrequestshistory")]
        public async Task<ActionResult<IList<LoanDto>>> GetBorrowerRequestsHistoryForCurrentUser()
        {
            var vm = await _loanService.GetBorrowRequestsHistoryForCurrentUser();

            return Ok(vm);
        }

        [HttpGet("lendrequestshistory")]
        public async Task<ActionResult<IList<LoanDto>>> GetLenderRequestsHistoryForCurrentUser()
        {
            var vm = await _loanService.GetLendRequestsHistoryForCurrentUser();

            return Ok(vm);
        }


        [HttpPut("cancelloanrequest")]
        public async Task<ActionResult<Result>> CancelLoanRequest([FromBody] LoanDto loanToUpdate)
        {
            var result = await _loanService.CancelLoanRequest(loanToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
