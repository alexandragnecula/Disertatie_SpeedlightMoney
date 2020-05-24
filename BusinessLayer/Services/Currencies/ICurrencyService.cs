using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;

namespace BusinessLayer.Services.Currencies
{
    public interface ICurrencyService
    {
        Task<Result> AddCurrency(CurrencyDto currencyToAdd);
        Task<Result> UpdateCurrency(CurrencyDto currencyToUpdate);
        Task<Result> DeleteCurrency(CurrencyDto currencyToDelete);
        Task<Result> RestoreCurrency(CurrencyDto currencyToRestore);
        Task<CurrencyDto> GetCurrencyById(long id);
        Task<IList<CurrencyDto>> GetAllCurrencies();
        Task<SelectItemVm> GetAllAsSelect(CurrencyDto currencyDto);

    }
}
