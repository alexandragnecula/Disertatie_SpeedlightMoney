using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;

namespace BusinessLayer.Services.DebtStatuses
{
    public interface IDebtStatusService
    {
        Task<Result> AddDebtStatus(DebtStatusDto debtStatusToAdd);
        Task<Result> UpdateDebtStatus(DebtStatusDto debtStatusToUpdate);
        Task<Result> DeleteDebtStatus(DebtStatusDto debtStatusToDelete);
        Task<Result> RestoreDebtStatus(DebtStatusDto debtStatusToRestore);
        Task<DebtStatusDto> GetDebtStatusById(long id);
        Task<IList<DebtStatusDto>> GetAllDebtStatuses();
        Task<SelectItemVm> GetAllAsSelect(DebtStatusDto debtStatusDto);
    }
}
