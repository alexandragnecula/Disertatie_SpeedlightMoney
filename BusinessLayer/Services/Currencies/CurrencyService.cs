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

namespace BusinessLayer.Services.Currencies
{
    public class CurrencyService : ICurrencyService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public CurrencyService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> AddCurrency(CurrencyDto currencyToAdd)
        {
            var entity = new Currency
            {
                CurrencyName = currencyToAdd.CurrencyName
            };

            await _context.Currencies.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Currency was created successfully");
        }

        public async Task<Result> UpdateCurrency(CurrencyDto currencyToUpdate)
        {
            var entity = await _context.Currencies
               .FirstOrDefaultAsync(x => x.Id == currencyToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid currency found" });
            }

            entity.CurrencyName = currencyToUpdate.CurrencyName;

            await _context.SaveChangesAsync();

            return Result.Success("Currency update was successful");
        }

        public async Task<Result> DeleteCurrency(CurrencyDto currencyToDelete)
        {
            var entity = await _context.Currencies               
                .FirstOrDefaultAsync(x => x.Id == currencyToDelete.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid currency found" });
            }

            entity.Deleted = true;

            await _context.SaveChangesAsync();

            return Result.Success("Currency was deleted");
        }

        public async Task<Result> RestoreCurrency(CurrencyDto currencyToRestore)
        {
            var entity = await _context.Currencies
                .FirstOrDefaultAsync(x => x.Id == currencyToRestore.Id && x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid currency found" });
            }

            entity.Deleted = false;
            entity.DeletedBy = null;
            entity.DeletedOn = null;

            await _context.SaveChangesAsync();

            return Result.Success("Currency was restored");
        }

        public async Task<CurrencyDto> GetCurrencyById(long id)
        {
            var entity = await _context.Currencies.FindAsync(id);

            return entity == null ? null : _mapper.Map<CurrencyDto>(entity);
        }

        public async Task<IList<CurrencyDto>> GetAllCurrencies()
        {
            List<CurrencyDto> currencies = await _context.Currencies
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return currencies;
        }

        public async Task<SelectItemVm> GetAllAsSelect(CurrencyDto currencyDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.Currencies
                    .Where(x => !x.Deleted)
                    .Select(x => new SelectItemDto { Label = x.CurrencyName, Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }
    }
}
