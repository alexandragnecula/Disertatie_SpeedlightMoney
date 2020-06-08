using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.DataContext;
using DataLayer.Entities;
using DataLayer.SharedInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public WalletService(DatabaseContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result> AddWallet(WalletDto walletToAdd)
        {
            var entity = new Wallet
            {
                TotalAmount = walletToAdd.TotalAmount,
                UserId = walletToAdd.UserId,
                CurrencyId = walletToAdd.CurrencyId
            };

            await _context.Wallets.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Wallet was created successfully");
        }

        public async Task<Result> UpdateWallet(WalletDto walletToUpdate)
        {
            var entity = await _context.Wallets
               .FirstOrDefaultAsync(x => x.Id == walletToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid wallet found" });
            }

            entity.TotalAmount = walletToUpdate.TotalAmount;
            entity.UserId = walletToUpdate.UserId;
            entity.CurrencyId = walletToUpdate.CurrencyId;

            await _context.SaveChangesAsync();

            return Result.Success("Wallet update was successful");
        }

        public async Task<Result> DeleteWallet(WalletDto walletToDelete)
        {
            var entity = await _context.Wallets
                 .FirstOrDefaultAsync(x => x.Id == walletToDelete.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid wallet found" });
            }

            entity.Deleted = true;

            await _context.SaveChangesAsync();

            return Result.Success("Wallet was deleted");
        }

        public async Task<Result> RestoreWallet(WalletDto walletToRestore)
        {
            var entity = await _context.Wallets
                .FirstOrDefaultAsync(x => x.Id == walletToRestore.Id && x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid wallet found" });
            }

            entity.Deleted = false;
            entity.DeletedBy = null;
            entity.DeletedOn = null;

            await _context.SaveChangesAsync();

            return Result.Success("Wallet was restored");
        }

        public async Task<WalletDto> GetWalletById(long id)
        {
            var entity = await _context.Wallets.FindAsync(id);

            return entity == null ? null : _mapper.Map<WalletDto>(entity);
        }

        public async Task<IList<WalletDto>> GetAllWallets()
        {
            List<WalletDto> wallets = await _context.Wallets
                 .OrderByDescending(x => x.CreatedOn)
                 .ProjectTo<WalletDto>(_mapper.ConfigurationProvider)
                 .ToListAsync();

            return wallets;
        }

        public async Task<IList<WalletDto>> GetWalletsForCurrentUser()
        {

            if (_currentUserService.UserId == null)
            {
                //de verificat castul, not ok
                return (IList<WalletDto>)Result.Failure(new List<string> { "No valid current user found" });
            }

            List<WalletDto> walletsForCurrentUser = await _context.Wallets
                .Where(x => x.UserId == _currentUserService.UserId)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<WalletDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return walletsForCurrentUser;
        }

        public async Task<SelectItemVm> GetAllAsSelect(WalletDto walletDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.Wallets
                    .Where(x => !x.Deleted)
                    .Select(x => new SelectItemDto { Label = x.TotalAmount.ToString(), Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }

        public async Task<SelectItemVm> GetWalletsForCurrentUserAsSelect(WalletDto walletDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.Wallets
                    .Where(x => x.UserId == _currentUserService.UserId && !x.Deleted)
                    .Select(x => new SelectItemDto { Label = x.Currency.CurrencyName + " " + x.TotalAmount.ToString(), Value = x.Id.ToString() })
                    .OrderByDescending(x => x.Label)
                    .ToListAsync()
            };

            return vm;
        }

        public async Task<Result> AddMoneyToWallet(WalletDto walletToUpdate)
        {
            var entity = await _context.Wallets.FirstOrDefaultAsync(x => x.Id == walletToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid wallet found" });
            }

            entity.TotalAmount += walletToUpdate.TotalAmount;

            await _context.SaveChangesAsync();

            return Result.Success("Money were added was successful");
        }

        public async Task<Result> SendMoney(WalletDto walletToUpdate)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var beneficiaryWallet = await _context.Wallets
                                        .Where(x => x.CurrencyId == walletToUpdate.CurrencyId)
                                        .FirstOrDefaultAsync(x => x.UserId == walletToUpdate.UserId && !x.Deleted);

                var currentUserWallet = await _context.Wallets
                                       .Where(x => x.CurrencyId == walletToUpdate.CurrencyId)
                                       .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId.Value && !x.Deleted);


                if (beneficiaryWallet == null)
                {
                    return Result.Failure(new List<string> { "No valid beneficiary wallet found" });
                }


                if (currentUserWallet.TotalAmount >= walletToUpdate.TotalAmount)
                {
                    currentUserWallet.TotalAmount -= walletToUpdate.TotalAmount;
                    beneficiaryWallet.TotalAmount += walletToUpdate.TotalAmount;                   
                }
                else
                {
                    return Result.Failure(new List<string> { "Insufficient funds"});
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();              

                return Result.Success("Transaction was successful");

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                throw new Exception(e.ToString());
            }
        }
    }
}