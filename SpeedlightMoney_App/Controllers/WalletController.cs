﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Wallets;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class WalletController : ControllerBase
    {
        IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("addwallet")]
        public async Task<IActionResult> AddWallet([FromBody] WalletDto walletToAdd)
        {
            if (walletToAdd.CurrencyId == 0)
            {
                return BadRequest("The currency is mandatory!");
            }
            var result = await _walletService.AddWallet(walletToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UpdateWallet([FromBody] WalletDto walletToUpdate)
        {
            if (walletToUpdate.CurrencyId == 0)
            {
                return BadRequest("The currency is mandatory!");
            }

            var result = await _walletService.UpdateWallet(walletToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{walletId}")]
        public async Task<ActionResult<Result>> DeletWallet(long walletId)
        {
            var result = await _walletService.DeleteWallet(new WalletDto { Id = walletId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("restore")]
        public async Task<ActionResult<Result>> RestoreWallet(long walletId)
        {
            var result = await _walletService.RestoreWallet(new WalletDto { Id = walletId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WalletDto>> GetWalletById(long id)
        {
            var vm = await _walletService.GetWalletById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid wallet was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<WalletDto>> GetAllWallets()
        {
            var vm = await _walletService.GetAllWallets();

            return Ok(vm);
        }


        [HttpGet("userwallets")]
        public async Task<ActionResult<WalletDto>> GetWalletsForCurrentUser()
        {
            var vm = await _walletService.GetWalletsForCurrentUser();

            return Ok(vm);
        }

        [HttpGet("walletsdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetWalletsDropdown()
        {
            var vm = await _walletService.GetAllAsSelect(new WalletDto());

            return Ok(vm);
        }

        [HttpGet("userwalletsdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetWalletsForCurrentUserDropdown()
        {
            var vm = await _walletService.GetWalletsForCurrentUserAsSelect(new WalletDto());

            return Ok(vm);
        }

        [HttpPut("addmoney")]
        public async Task<ActionResult<Result>> AddMoneyToWallet([FromBody] WalletDto walletToUpdate)
        {
            var result = await _walletService.AddMoneyToWallet(walletToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("sendmoney")]
        public async Task<ActionResult<Result>> SendMoney([FromBody] WalletDto walletToUpdate)
        {
            var result = await _walletService.SendMoney(walletToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("banktransfer")]
        public async Task<ActionResult<Result>> BankTransfer([FromBody] WalletDto walletToUpdate)
        {
            var result = await _walletService.BankTransfer(walletToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
