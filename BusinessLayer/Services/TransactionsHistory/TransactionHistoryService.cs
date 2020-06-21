using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.DataContext;
using DataLayer.Entities;
using DataLayer.SharedInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.TransactionsHistory
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public TransactionHistoryService(DatabaseContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result> AddTransactionHistory(TransactionHistoryDto transactionHistoryToAdd)
        {
            var entity = new TransactionHistory
            {
                Amount = transactionHistoryToAdd.Amount,
                SenderId = _currentUserService.UserId.Value,
                BeneficiarId = transactionHistoryToAdd.BeneficiarId,
                CurrencyId = transactionHistoryToAdd.CurrencyId
            };

            await _context.TransactionsHistory.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Transaction History was created successfully");
        }

        public async Task<Result> UpdateTransactionHistory(TransactionHistoryDto transactionHistoryToUpdate)
        {
            var entity = await _context.TransactionsHistory
                .FirstOrDefaultAsync(x => x.Id == transactionHistoryToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid transaction history found" });
            }

            entity.Amount = transactionHistoryToUpdate.Amount;
            entity.SenderId = transactionHistoryToUpdate.SenderId;
            entity.BeneficiarId = transactionHistoryToUpdate.BeneficiarId;

            await _context.SaveChangesAsync();

            return Result.Success("Transaction History update was successful");
        }

        public async Task<Result> DeleteTransactionHistory(TransactionHistoryDto transactionHistoryToDelete)
        {
            var entity = await _context.TransactionsHistory
                .FirstOrDefaultAsync(x => x.Id == transactionHistoryToDelete.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid transaction history found" });
            }

            entity.Deleted = true;

            await _context.SaveChangesAsync();

            return Result.Success("Transaction History was deleted");
        }

        public async Task<Result> RestoreTransactionHistory(TransactionHistoryDto transactionHistoryToRestore)
        {
            var entity = await _context.TransactionsHistory
               .FirstOrDefaultAsync(x => x.Id == transactionHistoryToRestore.Id && x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid transaction history found" });
            }

            entity.Deleted = false;
            entity.DeletedBy = null;
            entity.DeletedOn = null;

            await _context.SaveChangesAsync();

            return Result.Success("Transaction History was restored");
        }

        public async Task<IList<TransactionHistoryDto>> GetAllTransactionsHistory()
        {
            List<TransactionHistoryDto> transactionsHistory = await _context.TransactionsHistory
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<TransactionHistoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return transactionsHistory;
        }

        public async Task<TransactionHistoryDto> GetTransactionHistoryById(long id)
        {
            var entity = await _context.TransactionsHistory.FindAsync(id);

            return entity == null ? null : _mapper.Map<TransactionHistoryDto>(entity);
        }

        public async Task<IList<TransactionHistoryDto>> GetTransactionsHistoryForCurrentUser()
        {
            if (_currentUserService.UserId == null)
            {
                //de verificat castul, not ok
                return (IList<TransactionHistoryDto>)Result.Failure(new List<string> { "No valid transaction history found" });
            }

            List<TransactionHistoryDto> transactionsHistoryForCurrentUser = await _context.TransactionsHistory
                .Where(x => x.SenderId == _currentUserService.UserId || x.BeneficiarId == _currentUserService.UserId)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<TransactionHistoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return transactionsHistoryForCurrentUser;
        }

       

    }
}
