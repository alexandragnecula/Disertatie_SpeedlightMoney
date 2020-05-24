using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> AddUser([FromBody] CurrencyDto currencyToAdd)
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
        public async Task<ActionResult<Result>> UpdateCity([FromBody] CurrencyDto currencyToUpdate)
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
    }
}
