using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;

namespace BusinessLayer.Services.LoanStatuses
{
    public interface ILoanStatusService
    {
        Task<Result> AddLoanStatus(LoanStatusDto loanStatusToAdd);
        Task<Result> UpdateLoanStatus(LoanStatusDto loanStatusToUpdate);
        Task<Result> DeleteLoanStatus(LoanStatusDto loanStatusToDelete);
        Task<Result> RestoreLoanStatus(LoanStatusDto loanStatusToRestore);
        Task<LoanStatusDto> GetLoanStatusById(long id);
        Task<IList<LoanStatusDto>> GetAllLoanStatuses();
        Task<SelectItemVm> GetAllAsSelect(LoanStatusDto loanStatusDto);
    }
}
