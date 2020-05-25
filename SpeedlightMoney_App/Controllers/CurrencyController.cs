using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Currencies;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CurrencyController : ControllerBase
    {
        ICurrencyService _currencyService;


        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpPost("addcurrency")]
        public async Task<IActionResult> AddCurrency([FromBody] CurrencyDto currencyToAdd)
        {
            if(currencyToAdd.CurrencyName == null)
            {
                return BadRequest("The currency name is mandatory!");
            }
            var result = await _currencyService.AddCurrency(currencyToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UpdateCurrecy([FromBody] CurrencyDto currencyToUpdate)
        {
            if (currencyToUpdate.CurrencyName == null)
            {
                return BadRequest("The currency name is mandatory!");
            }

            var result = await _currencyService.UpdateCurrency(currencyToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{currencyId}")]
        public async Task<ActionResult<Result>> DeleteCurrency(long currencyId)
        {
            var result = await _currencyService.DeleteCurrency(new CurrencyDto { Id = currencyId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("restore")]
        public async Task<ActionResult<Result>> RestoreCurrency(long currencyId)
        {
            var result = await _currencyService.RestoreCurrency(new CurrencyDto { Id = currencyId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyDto>> GetCurrencyById(long id)
        {
            var vm = await _currencyService.GetCurrencyById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid currency was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<CurrencyDto>> GetAllCurrencies()
        {
            var vm = await _currencyService.GetAllCurrencies();

            return Ok(vm);
        }

        [HttpGet("currenciesdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetCurrenciesDropdown()
        {
            var vm = await _currencyService.GetAllAsSelect(new CurrencyDto());

            return Ok(vm);
        }
    }
}
