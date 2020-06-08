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

namespace BusinessLayer.Services.Debts
{
    public class DebtService : IDebtService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public DebtService(DatabaseContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result> AddDebt(DebtDto debtToAdd)
        {
            var entity = new Debt
            {
                LoanId = debtToAdd.LoanId,
                DebtStatusId = debtToAdd.DebtStatusId
            };

            await _context.Debts.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Debt was created successfully");
        }

        public async Task<Result> UpdateDebt(DebtDto debtToUpdate)
        {
            var entity = await _context.Debts
              .FirstOrDefaultAsync(x => x.Id == debtToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid debt found" });
            }

            entity.LoanId = debtToUpdate.LoanId;
            entity.DebtStatusId = debtToUpdate.DebtStatusId;

            await _context.SaveChangesAsync();

            return Result.Success("Debt update was successful");
        }

        public async Task<Result> DeleteDebt(DebtDto debtToDelete)
        {
            var entity = await _context.Debts
                .FirstOrDefaultAsync(x => x.Id == debtToDelete.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid debt found" });
            }

            entity.Deleted = true;

            await _context.SaveChangesAsync();

            return Result.Success("Debt was deleted");
        }

        public async Task<Result> RestoreDebt(DebtDto debtToRestore)
        {
            var entity = await _context.Debts
                .FirstOrDefaultAsync(x => x.Id == debtToRestore.Id && x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid debt found" });
            }

            entity.Deleted = false;
            entity.DeletedBy = null;
            entity.DeletedOn = null;

            await _context.SaveChangesAsync();

            return Result.Success("Debt was restored");
        }

        public async Task<DebtDto> GetDebtById(long id)
        {
            var entity = await _context.Debts.FindAsync(id);

            return entity == null ? null : _mapper.Map<DebtDto>(entity);
        }

        public async Task<IList<DebtDto>> GetAllDebts()
        {
            List<DebtDto> debts = await _context.Debts
                 .OrderByDescending(x => x.CreatedOn)
                 .ProjectTo<DebtDto>(_mapper.ConfigurationProvider)
                 .ToListAsync();

            return debts;
        }

        public async Task<SelectItemVm> GetAllAsSelect(DebtDto debtDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.Debts
                    .Where(x => !x.Deleted)
                    .Select(x => new SelectItemDto { Label = x.Loan.Description, Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }

        public async Task<IList<DebtDto>> GetDebtsForCurrentUser()
        {
            List<DebtDto> debts = await _context.Debts
                .Where(x => x.Loan.BorrowerId == (_currentUserService.UserId.HasValue ? _currentUserService.UserId.Value : 0))
                .Include(x => x.Loan)
                .Include(x=> x.DebtStatus)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<DebtDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return debts;
        }

        public async Task<IList<DebtDto>> GetCreditsForCurrentUser()
        {
            List<DebtDto> debts = await _context.Debts
                .Where(x => x.Loan.LenderId == _currentUserService.UserId.Value)
                .Include(x => x.Loan)
                .Include(x => x.DebtStatus)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<DebtDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return debts;
        }


    }
}
