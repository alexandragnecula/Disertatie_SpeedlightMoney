using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Services.TransactionsHistory;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TransactionHistoryController : ControllerBase
    {
        ITransactionHistoryService _transactionHistoryService;

        public TransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }

        [HttpPost("addtransactionhistory")]
        public async Task<IActionResult> AddTransactionHistory([FromBody] TransactionHistoryDto transactionHistoryToAdd)
        {
            if (transactionHistoryToAdd.CurrencyId == 0)
            {
                return BadRequest("The currency is mandatory!");
            }
            var result = await _transactionHistoryService.AddTransactionHistory(transactionHistoryToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UpdateTransactionHistory([FromBody] TransactionHistoryDto transactionHistoryToUpdate)
        {
            if (transactionHistoryToUpdate.CurrencyId == 0)
            {
                return BadRequest("The currency is mandatory!");
            }

            var result = await _transactionHistoryService.UpdateTransactionHistory(transactionHistoryToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{transactionHistoryId}")]
        public async Task<ActionResult<Result>> DeleteTransactionHistory(long transactionHistoryId)
        {
            var result = await _transactionHistoryService.DeleteTransactionHistory(new TransactionHistoryDto { Id = transactionHistoryId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("restore")]
        public async Task<ActionResult<Result>> RestoreTransactionHistory(long transactionHistoryId)
        {
            var result = await _transactionHistoryService.RestoreTransactionHistory(new TransactionHistoryDto { Id = transactionHistoryId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionHistoryDto>> GetTransactionHistoryById(long id)
        {
            var vm = await _transactionHistoryService.GetTransactionHistoryById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid transaction history was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<TransactionHistoryDto>> GetAllTransactionsHistory()
        {
            var vm = await _transactionHistoryService.GetAllTransactionsHistory();

            return Ok(vm);
        }


        [HttpGet("usertransactionshistory")]
        public async Task<ActionResult<WalletDto>> GetTransactionsHistoryForCurrentUser()
        {
            var vm = await _transactionHistoryService.GetTransactionsHistoryForCurrentUser();

            return Ok(vm);
        }
    }
}
