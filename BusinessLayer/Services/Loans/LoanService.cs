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

namespace BusinessLayer.Services.Loans
{
    public class LoanService : ILoanService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public LoanService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> AddLoan(LoanDto loanToAdd)
        {
            var entity = new Loan
            {
                Description = loanToAdd.Description,
                Amount = loanToAdd.Amount,
                BorrowDate = loanToAdd.BorrowDate,
                ReturnDate = loanToAdd.ReturnDate,
                DueDate = loanToAdd.DueDate,
                BorrowerId = loanToAdd.BorrowerId,
                LenderId = loanToAdd.LenderId,
                CurrencyId = loanToAdd.CurrencyId,
                LoanStatusId = loanToAdd.LoanStatusId,
                TermId = loanToAdd.TermId
            };

            await _context.Loans.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Loan was created successfully");
        }

        public async Task<Result> UpdateLoan(LoanDto loanToUpdate)
        {
            var entity = await _context.Loans
               .FirstOrDefaultAsync(x => x.Id == loanToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid loan found" });
            }

            entity.Description = loanToUpdate.Description;
            entity.Amount = loanToUpdate.Amount;
            entity.BorrowDate = loanToUpdate.BorrowDate;
            entity.ReturnDate = loanToUpdate.ReturnDate;
            entity.DueDate = loanToUpdate.DueDate;
            entity.BorrowerId = loanToUpdate.BorrowerId;
            entity.LenderId = loanToUpdate.LenderId;
            entity.CurrencyId = loanToUpdate.CurrencyId;
            entity.LoanStatusId = loanToUpdate.LoanStatusId;
            entity.TermId = loanToUpdate.TermId;          

            await _context.SaveChangesAsync();

            return Result.Success("Loan update was successful");
        }

        public async Task<Result> DeleteLoan(LoanDto loanToDelete)
        {
            var entity = await _context.Loans
                .FirstOrDefaultAsync(x => x.Id == loanToDelete.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid loan found" });
            }

            entity.Deleted = true;

            await _context.SaveChangesAsync();

            return Result.Success("Loan was deleted");
        }

        public async Task<Result> RestoreLoan(LoanDto loanToRestore)
        {
            var entity = await _context.Loans
                .FirstOrDefaultAsync(x => x.Id == loanToRestore.Id && x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid loan found" });
            }

            entity.Deleted = false;
            entity.DeletedBy = null;
            entity.DeletedOn = null;

            await _context.SaveChangesAsync();

            return Result.Success("Loan was restored");
        }

        public async Task<LoanDto> GetLoanById(long id)
        {
            var entity = await _context.Loans.FindAsync(id);

            return entity == null ? null : _mapper.Map<LoanDto>(entity);
        }

        public async Task<IList<LoanDto>> GetAllLoans()
        {
            List<LoanDto> loans = await _context.Currencies
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<LoanDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return loans;
        }
      
        public async Task<SelectItemVm> GetAllAsSelect(LoanDto loanDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.Loans
                    .Where(x => !x.Deleted)
                    .Select(x => new SelectItemDto { Label = x.Description, Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }
    }
}
