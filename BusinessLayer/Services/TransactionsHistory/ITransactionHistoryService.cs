using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.Entities;

namespace BusinessLayer.Services.TransactionsHistory
{
    public interface ITransactionHistoryService
    {
        Task<Result> AddTransactionHistory(TransactionHistoryDto transactionHistoryToAdd);
        Task<Result> UpdateTransactionHistory(TransactionHistoryDto transactionHistoryToUpdate);
        Task<Result> DeleteTransactionHistory(TransactionHistoryDto transactionHistoryToDelete);
        Task<Result> RestoreTransactionHistory(TransactionHistoryDto transactionHistoryToRestore);
        Task<TransactionHistoryDto> GetTransactionHistoryById(long id);
        Task<IList<TransactionHistoryDto>> GetAllTransactionsHistory();
        Task<IList<TransactionHistoryDto>> GetTransactionsHistoryForCurrentUser();
    }
}
