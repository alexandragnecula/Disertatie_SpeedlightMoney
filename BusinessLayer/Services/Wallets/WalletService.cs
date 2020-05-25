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
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public WalletService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<SelectItemVm> GetAllAsSelect(WalletDto walletDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.Wallets
                    .Where(x => !x.Deleted)
                    .Select(x => new SelectItemDto { Label = x.Currency.CurrencyName, Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }     
    }
}
