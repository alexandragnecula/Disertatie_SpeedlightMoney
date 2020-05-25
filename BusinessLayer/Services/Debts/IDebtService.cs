using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;

namespace BusinessLayer.Services.Debts
{
    public interface IDebtService
    {
        Task<Result> AddDebt(DebtDto debtToAdd);
        Task<Result> UpdateDebt(DebtDto cdebtToUpdate);
        Task<Result> DeleteDebt(DebtDto debtToDelete);
        Task<Result> RestoreDebt(DebtDto debtToRestore);
        Task<DebtDto> GetDebtById(long id);
        Task<IList<DebtDto>> GetAllDebts();
        Task<SelectItemVm> GetAllAsSelect(DebtDto debtDto);
    }
}
