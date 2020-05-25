using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Terms;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TermController : ControllerBase
    {
        ITermService _termService;

        public TermController(ITermService termService)
        {
            _termService = termService;
        }

        [HttpPost("addterm")]
        public async Task<IActionResult> AddTerm([FromBody] TermDto termToAdd)
        {
            if (termToAdd.TermName == null)
            {
                return BadRequest("The term name is mandatory!");
            }
            var result = await _termService.AddTerm(termToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UpdateTerm([FromBody] TermDto termToUpdate)
        {
            if (termToUpdate.TermName == null)
            {
                return BadRequest("The term name is mandatory!");
            }

            var result = await _termService.UpdateTerm(termToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{termId}")]
        public async Task<ActionResult<Result>> DeleteTerm(long termId)
        {
            var result = await _termService.DeleteTerm(new TermDto { Id = termId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("restore")]
        public async Task<ActionResult<Result>> RestoreTerm(long termId)
        {
            var result = await _termService.RestoreTerm(new TermDto { Id = termId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TermDto>> GetTermById(long id)
        {
            var vm = await _termService.GetTermById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid term was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<TermDto>> GetAllTerms()
        {
            var vm = await _termService.GetAllTerms();

            return Ok(vm);
        }

        [HttpGet("termsdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetTermsDropdown()
        {
            var vm = await _termService.GetAllAsSelect(new TermDto());

            return Ok(vm);
        }
    }
}
