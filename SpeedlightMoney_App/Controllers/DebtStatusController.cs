using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.DebtStatuses;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class DebtStatusController : ControllerBase
    {
        IDebtStatusService _debtStatusService;

        public DebtStatusController(IDebtStatusService debtStatusService) 
        {
            _debtStatusService = debtStatusService;
        }

        [HttpPost("adddebtstatus")]
        public async Task<IActionResult> AddDebtStatus([FromBody] DebtStatusDto debtStatusToAdd)
        {
            if (debtStatusToAdd.DebtStatusName == null)
            {
                return BadRequest("The debt status name is mandatory!");
            }
            var result = await _debtStatusService.AddDebtStatus(debtStatusToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UpdateDebtStatus([FromBody] DebtStatusDto debtStatusToUpdate)
        {
            if (debtStatusToUpdate.DebtStatusName == null)
            {
                return BadRequest("The debt status name is mandatory!");
            }

            var result = await _debtStatusService.UpdateDebtStatus(debtStatusToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{debtStatusId}")]
        public async Task<ActionResult<Result>> DeleteDebtStatus(long debtStatusId)
        {
            var result = await _debtStatusService.DeleteDebtStatus(new DebtStatusDto { Id = debtStatusId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("restore")]
        public async Task<ActionResult<Result>> RestoreDebtStatus(long debtStatusId)
        {
            var result = await _debtStatusService.RestoreDebtStatus(new DebtStatusDto { Id = debtStatusId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DebtStatusDto>> GetDebtStatusById(long id)
        {
            var vm = await _debtStatusService.GetDebtStatusById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid debt status was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<DebtStatusDto>> GetAllDebtStatuses()
        {
            var vm = await _debtStatusService.GetAllDebtStatuses();

            return Ok(vm);
        }

        [HttpGet("debtstatusesdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetDebtStatusesDropdown()
        {
            var vm = await _debtStatusService.GetAllAsSelect(new DebtStatusDto());

            return Ok(vm);
        }
    }
}
