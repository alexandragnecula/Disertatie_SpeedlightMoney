using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Wallets;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.DataContext;
using DataLayer.Entities;
using DataLayer.SharedInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Loans
{
    public class LoanService : ILoanService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWalletService _walletService;

        public LoanService(DatabaseContext context, IMapper mapper, ICurrentUserService currentUserService,
                            IWalletService walletService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _walletService = walletService;
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
            List<LoanDto> loans = await _context.Loans
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

        public async Task<Result> RequestLoan(LoanDto loanToAdd)
        {
            var entity = new Loan
            {
                Description = loanToAdd.Description,
                Amount = loanToAdd.Amount,
                BorrowDate = null,
                ReturnDate = null,
                DueDate = null,
                BorrowerId = _currentUserService.UserId.Value, 
                LenderId = loanToAdd.LenderId,
                CurrencyId = loanToAdd.CurrencyId,
                LoanStatusId = 3,
                TermId = loanToAdd.TermId
            };

            await _context.Loans.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Loan was requested successfully");
        }

        public async Task<Result>  ApproveLoan(LoanDto loanToUpdate)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _context.Loans
                                           .FirstOrDefaultAsync(x => x.Id == loanToUpdate.Id && !x.Deleted);

                if (entity == null)
                {
                    return Result.Failure(new List<string> { "No valid loan found" });
                }

                entity.BorrowDate = loanToUpdate.LoanStatusId == 1 ? DateTime.Now : (DateTime?)null;
                entity.LoanStatusId = loanToUpdate.LoanStatusId;
                entity.DueDate = loanToUpdate.LoanStatusId == 1 ?
                                  (entity.TermId == 1 ? (DateTime.Now).AddDays(14)
                                                        : (entity.TermId == 2 ? (DateTime.Now).AddDays(30)
                                                                                 : (entity.TermId == 3 ? (DateTime.Now).AddDays(90)
                                                                                                           : (entity.TermId == 4) ? (DateTime.Now).AddDays(180)
                                                                                                                                     : (DateTime.Now).AddDays(365)))) : (DateTime?)null;

                if (loanToUpdate.LoanStatusId == 1)
                {
                    //lender wallets
                    IList<WalletDto> walletsForCurrentUser = await _walletService.GetWalletsForCurrentUser();
                   
                    //borrower wallets
                    List<WalletDto> walletsForBorrower = await _context.Wallets
                                                               .Where(x => x.UserId == loanToUpdate.BorrowerId )
                                                               .ProjectTo<WalletDto>(_mapper.ConfigurationProvider)
                                                               .ToListAsync();

                    if (walletsForCurrentUser == null && walletsForBorrower == null)
                    {
                        return Result.Failure(new List<string> { "No valid wallet found" });
                    }

                    foreach (var currentUserWallet in walletsForCurrentUser)
                    {
                        //    var wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.Id == walletToUpdate.Id && !x.Deleted);
                       
                        if (currentUserWallet.TotalAmount >= loanToUpdate.Amount)
                        {
                            //Update RON
                            if (loanToUpdate.CurrencyId == 1)
                            {
                                walletsForCurrentUser[1].TotalAmount -= loanToUpdate.Amount;
                                walletsForBorrower[1].TotalAmount += loanToUpdate.Amount;
                            }

                            //Update EUR
                            if (loanToUpdate.CurrencyId == 2)
                            {
                                walletsForCurrentUser[0].TotalAmount -= loanToUpdate.Amount;
                                walletsForBorrower[0].TotalAmount += loanToUpdate.Amount;
                            }
                        }
                        else
                        {
                            return Result.Failure(new List<string> { "Inssuficient funds" });
                        }
                    }

                    var debt = new Debt
                    {
                        LoanId = loanToUpdate.Id,
                        DebtStatusId = 1
                    };
                    await _context.Debts.AddAsync(debt);
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return Result.Success("Loan was successfully approved!");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                throw new Exception(e.ToString());
            }

        }

        public async Task<IList<LoanDto>> GetBorrowRequestsForCurrentUser()
        {
            List<LoanDto> loans = await _context.Loans
                .Where(x => x.BorrowerId == _currentUserService.UserId.Value)
                .Include(x => x.Borrower)
                .Include(x => x.Lender)
                .Include(x => x.Currency)
                .Include(x => x.Term)
                .Include(x => x.LoanStatus)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<LoanDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return loans;
        }

        public async Task<IList<LoanDto>> GetLendRequestsForCurrentUser()
        {
            List<LoanDto> loans = await _context.Loans
                .Where(x => x.LenderId == _currentUserService.UserId.Value)
                .Include(x => x.Borrower)
                .Include(x => x.Lender)
                .Include(x => x.Currency)
                .Include(x => x.Term)
                .Include(x => x.LoanStatus)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<LoanDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return loans;
        }
    }
}
