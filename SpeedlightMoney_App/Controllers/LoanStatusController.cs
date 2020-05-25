using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Currencies;
using BusinessLayer.Services.LoanStatuses;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class LoanStatusController : ControllerBase
    {
        ILoanStatusService _loanStatusService;

        public LoanStatusController(ILoanStatusService loanStatusService)
        {
            _loanStatusService = loanStatusService;
        }

        [HttpPost("addloanstatus")]
        public async Task<IActionResult> AddLoanStatus([FromBody] LoanStatusDto loanStatusToAdd)
        {
            if (loanStatusToAdd.LoanStatusName == null)
            {
                return BadRequest("The loan status name is mandatory!");
            }
            var result = await _loanStatusService.AddLoanStatus(loanStatusToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UpdateLoanStatus([FromBody] LoanStatusDto loanStatusToUpdate)
        {
            if (loanStatusToUpdate.LoanStatusName == null)
            {
                return BadRequest("The loan status name is mandatory!");
            }

            var result = await _loanStatusService.UpdateLoanStatus(loanStatusToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{loanStatusId}")]
        public async Task<ActionResult<Result>> DeleteLoanStatus(long loanStatusId)
        {
            var result = await _loanStatusService.DeleteLoanStatus(new LoanStatusDto { Id = loanStatusId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("restore")]
        public async Task<ActionResult<Result>> RestoreLoanStatus(long loanStatusId)
        {
            var result = await _loanStatusService.RestoreLoanStatus(new LoanStatusDto { Id = loanStatusId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanStatusDto>> GetLoanStatusById(long id)
        {
            var vm = await _loanStatusService.GetLoanStatusById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid loan status was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<LoanStatusDto>> GetAllLoanStatuses()
        {
            var vm = await _loanStatusService.GetAllLoanStatuses();

            return Ok(vm);
        }

        [HttpGet("loanstatusesdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetLoanStatusesDropdown()
        {
            var vm = await _loanStatusService.GetAllAsSelect(new LoanStatusDto());

            return Ok(vm);
        }
    }
}
