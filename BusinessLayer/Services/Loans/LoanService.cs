﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Common.Constants;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Wallets;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.DataContext;
using DataLayer.Entities;
using DataLayer.SharedInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Loans
{
    public class LoanService : ILoanService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWalletService _walletService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoanService(DatabaseContext context, IMapper mapper, ICurrentUserService currentUserService,
                            IWalletService walletService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _walletService = walletService;
            _userManager = userManager;
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
            entity.LenderId = loanToUpdate.LenderId;
            entity.CurrencyId = loanToUpdate.CurrencyId;
            entity.TermId = loanToUpdate.TermId;          

            await _context.SaveChangesAsync();

            return Result.Success("Loan request updated successfully");
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
            var walletForCurrentUserRON = await _context.Wallets.Where(x => x.UserId == _currentUserService.UserId)
                                          .FirstOrDefaultAsync(x => x.CurrencyId == 1);
            var walletForCurrentUserEUR = await _context.Wallets.Where(x => x.UserId == _currentUserService.UserId)
                                          .FirstOrDefaultAsync(x => x.CurrencyId == 2);

            if(walletForCurrentUserRON.TotalAmount < 30 && walletForCurrentUserEUR.TotalAmount < 6)
            {
                return Result.Failure(new List<string> { "Insufficient funds. You must have at least 30 RON/€6 in your wallet to request a loan!" });
            }

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


            if(entity.CurrencyId == 1)
            {
                if (entity.Amount > 25000)
                {
                    return Result.Failure(new List<string> { "You cannot borrow more than 25.000 RON. Please request another amount." });
                }
            }
            else if(entity.CurrencyId == 2)
            {
                if (entity.Amount > 5000)
                {
                    return Result.Failure(new List<string> { "You cannot borrow more than €5.0000. Please request another amount." });
                }
            }
            
            await _context.Loans.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Loan was requested successfully");
        }

        public async Task<Result>  ManageLoan(LoanDto loanToUpdate)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _context.Loans
                                           .Include(x => x.Lender)
                                           .FirstOrDefaultAsync(x => x.Id == loanToUpdate.Id && !x.Deleted);

                if (entity == null)
                {
                    return Result.Failure(new List<string> { "No valid loan found" });
                }

                entity.Deleted = true;
                entity.BorrowDate = loanToUpdate.LoanStatusId == 1 ? DateTime.Now : (DateTime?)null;
                entity.LoanStatusId = loanToUpdate.LoanStatusId;
                entity.DueDate = loanToUpdate.LoanStatusId == 1 ?
                                  (entity.TermId == 1 ? (DateTime.Now).AddDays(14)
                                                        : (entity.TermId == 2 ? (DateTime.Now).AddDays(30)
                                                                                 : (entity.TermId == 3 ? (DateTime.Now).AddDays(90)
                                                                                                           : (entity.TermId == 4) ? (DateTime.Now).AddDays(180)
                                                                                                                                     : (DateTime.Now).AddDays(365)))) : (DateTime?)null;
                await _context.SaveChangesAsync();

                if (loanToUpdate.LoanStatusId == 1)
                {
                    //create debt for borrower
                    var debt = new Debt
                    {
                        LoanId = loanToUpdate.Id,
                        DebtStatusId = 1                 
                    };

                    var wallletForCurentUser = await _context.Wallets.Where(x => x.UserId == loanToUpdate.LenderId)
                                                                        .FirstOrDefaultAsync(x => x.CurrencyId == loanToUpdate.CurrencyId);

                    var walletForBorrower = await _context.Wallets.Where(x => x.UserId == loanToUpdate.BorrowerId)
                                                                     .FirstOrDefaultAsync(x => x.CurrencyId == loanToUpdate.CurrencyId);


                    bool isPremium = await _userManager.IsInRoleAsync(entity.Lender, RoleConstants.Premium);
                    bool isUltimate = await _userManager.IsInRoleAsync(entity.Lender, RoleConstants.Ultimate);


                    //add amount to borrower wallet and withdraw amount from lender
                    if (loanToUpdate.CurrencyId == wallletForCurentUser.CurrencyId)
                    {
                        if (wallletForCurentUser.TotalAmount >= loanToUpdate.Amount)
                        {
                            if (isUltimate)
                            {
                                if (loanToUpdate.CurrencyId == 1)
                                {
                                    wallletForCurentUser.TotalAmount -= 1;
                                }
                                else if (loanToUpdate.CurrencyId == 2)
                                {
                                    wallletForCurentUser.TotalAmount -= 0.2;
                                }
                            }
                            else if (isPremium)
                            {
                                if (loanToUpdate.CurrencyId == 1)
                                {
                                    wallletForCurentUser.TotalAmount -= 1.5;
                                }
                                else if (loanToUpdate.CurrencyId == 2)
                                {
                                    wallletForCurentUser.TotalAmount -= 0.3;
                                }
                            }
                         
                            wallletForCurentUser.TotalAmount -= loanToUpdate.Amount;
                            walletForBorrower.TotalAmount += loanToUpdate.Amount;
                           
                        }
                        else
                        {
                            return Result.Failure(new List<string> { "Insuficient funds" });
                        }
                    }

                    await _context.Debts.AddAsync(debt);

                    await transaction.CommitAsync();

                    await _context.SaveChangesAsync();

                    return Result.Success("Loan was successfully approved");
                }
                else if(loanToUpdate.LoanStatusId == 2)
                {
                    await transaction.CommitAsync();

                    return Result.Success("Loan was successfully rejected");
                }
                else
                {
                    return Result.Success("Something went wrong. Please contact your administrator");
                }

                
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
                .Where(x => x.BorrowerId == _currentUserService.UserId.Value && !x.Deleted)
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
                .Where(x => x.LenderId == _currentUserService.UserId.Value && !x.Deleted)
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

        public async Task<IList<LoanDto>> GetBorrowRequestsHistoryForCurrentUser()
        {
            List<LoanDto> loans = await _context.Loans
                .Where(x => x.BorrowerId == _currentUserService.UserId.Value && x.Deleted)
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

        public async Task<IList<LoanDto>> GetLendRequestsHistoryForCurrentUser()
        {
            List<LoanDto> loans = await _context.Loans
                .Where(x => x.LenderId == _currentUserService.UserId.Value && x.Deleted)
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

        public async Task<Result> CancelLoanRequest(LoanDto loanToUpdate)
        {
            var entity = await _context.Loans.FirstOrDefaultAsync(x => x.Id == loanToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid loan found" });
            }

            if(entity.LoanStatusId == 1 || entity.LoanStatusId != 2)
            {
                entity.Deleted = true;
                entity.LoanStatusId = 5;


                await _context.SaveChangesAsync();
            }
            else
            {
                return Result.Failure(new List<string> { "The loan has already been managed by the lender" });
            }

            return Result.Success("Loan request was successfully canceled");
        }



    }
}
