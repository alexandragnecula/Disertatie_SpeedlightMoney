using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Debts;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DebtController : ControllerBase
    {
        IDebtService _debtService;
        public DebtController(IDebtService debtService)
        {
            _debtService = debtService;
        }

        [HttpPost("adddebt")]
        public async Task<IActionResult> AddUser([FromBody] DebtDto debtToAdd)
        {
            if (debtToAdd.DebtStatusName == null)
            {
                return BadRequest("The status is mandatory!");
            }

            if (debtToAdd.LoanAmount == 0)
            {
                return BadRequest("The amount is mandatory!");
            }

            var result = await _debtService.AddDebt(debtToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UpdateCity([FromBody] DebtDto debtToUpdate)
        {
            if (debtToUpdate.DebtStatusName == null)
            {
                return BadRequest("The status is mandatory!");
            }

            if (debtToUpdate.LoanAmount == 0)
            {
                return BadRequest("The amount is mandatory!");
            }

            var result = await _debtService.UpdateDebt(debtToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{debtId}")]
        public async Task<ActionResult<Result>> DeleteDebt(long debtId)
        {
            var result = await _debtService.DeleteDebt(new DebtDto { Id = debtId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("restore")]
        public async Task<ActionResult<Result>> RestoreDebt(long debtId)
        {
            var result = await _debtService.RestoreDebt(new DebtDto { Id = debtId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DebtDto>> GetDebtById(long id)
        {
            var vm = await _debtService.GetDebtById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid debt was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<DebtDto>> GetAllDebts()
        {
            var vm = await _debtService.GetAllDebts();

            return Ok(vm);
        }

        [HttpGet("debtsdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetDebtsDropdown()
        {
            var vm = await _debtService.GetAllAsSelect(new DebtDto());

            return Ok(vm);
        }
    }
}
