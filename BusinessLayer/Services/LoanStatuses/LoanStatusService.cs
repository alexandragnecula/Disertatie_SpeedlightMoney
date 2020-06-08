using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Loans;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.DataContext;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.LoanStatuses
{
    public class LoanStatusService :ILoanStatusService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ILoanService _loanService;

        public LoanStatusService(DatabaseContext context, IMapper mapper, ILoanService loanService)
        {
            _context = context;
            _mapper = mapper;
            _loanService = loanService;
        }

        public async Task<Result> AddLoanStatus(LoanStatusDto loanStatusToAdd)
        {

            var entity = new LoanStatus
            {
                LoanStatusName = loanStatusToAdd.LoanStatusName
            };

            await _context.LoanStatuses.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Loan status was created successfully");
        }

        public async Task<Result> UpdateLoanStatus(LoanStatusDto loanStatusToUpdate)
        {
            var entity = await _context.LoanStatuses
               .FirstOrDefaultAsync(x => x.Id == loanStatusToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid loan status found" });
            }

            entity.LoanStatusName = loanStatusToUpdate.LoanStatusName;

            await _context.SaveChangesAsync();

            return Result.Success("Loan status update was successful");
        }

        public async Task<Result> DeleteLoanStatus(LoanStatusDto loanStatusToDelete)
        {
            var entity = await _context.LoanStatuses
                .FirstOrDefaultAsync(x => x.Id == loanStatusToDelete.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid loan status found" });
            }

            entity.Deleted = true;

            await _context.SaveChangesAsync();

            return Result.Success("Loan status was deleted");
        }

        public async Task<Result> RestoreLoanStatus(LoanStatusDto loanStatusToRestore)
        {
            var entity = await _context.LoanStatuses
               .FirstOrDefaultAsync(x => x.Id == loanStatusToRestore.Id && x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid loan status found" });
            }

            entity.Deleted = false;
            entity.DeletedBy = null;
            entity.DeletedOn = null;

            await _context.SaveChangesAsync();

            return Result.Success("Loan status was restored");
        }

        public async Task<LoanStatusDto> GetLoanStatusById(long id)
        {
            var entity = await _context.LoanStatuses.FindAsync(id);

            return entity == null ? null : _mapper.Map<LoanStatusDto>(entity);
        }

        public async Task<IList<LoanStatusDto>> GetAllLoanStatuses()
        {
            List<LoanStatusDto> loanStatuses = await _context.LoanStatuses
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<LoanStatusDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return loanStatuses;
        }

        public async Task<SelectItemVm> GetAllAsSelect(LoanStatusDto loanStatusDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.LoanStatuses
                    .Where(x => !x.Deleted && x.LoanStatusName != "Pending")
                    .Select(x => new SelectItemDto { Label = x.LoanStatusName, Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }
    }
}
