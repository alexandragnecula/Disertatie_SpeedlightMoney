using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;

namespace BusinessLayer.Services.Loans
{
    public interface ILoanService
    {
        Task<Result> AddLoan(LoanDto loanToAdd);
        Task<Result> UpdateLoan(LoanDto loanToUpdate);
        Task<Result> DeleteLoan(LoanDto loanToDelete);
        Task<Result> RestoreLoan(LoanDto loanToRestore);
        Task<LoanDto> GetLoanById(long id);
        Task<IList<LoanDto>> GetAllLoans();
        Task<SelectItemVm> GetAllAsSelect(LoanDto loanDto);
        Task<Result> RequestLoan(LoanDto loanToAdd);
        Task<Result> ManageLoan(LoanDto loanToAdd);
        Task<IList<LoanDto>> GetBorrowRequestsForCurrentUser();
        Task<IList<LoanDto>> GetLendRequestsForCurrentUser();
        Task<IList<LoanDto>> GetBorrowRequestsHistoryForCurrentUser();
        Task<IList<LoanDto>> GetLendRequestsHistoryForCurrentUser();
    }
}
