using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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

        public async Task<IList<DebtDto>> GetDebts()
        {
            List<DebtDto> debts = await _context.Debts
                .Include(x => x.Loan)
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
                .Where(x => x.Loan.BorrowerId == (_currentUserService.UserId.HasValue ? _currentUserService.UserId.Value : 0) && !x.Deleted)
                .Include(x => x.Loan)
                .Include(x => x.DebtStatus)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<DebtDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return debts;
        }

        public async Task<IList<DebtDto>> GetCreditsForCurrentUser()
        {
            List<DebtDto> debts = await _context.Debts
                .Where(x => x.Loan.LenderId == _currentUserService.UserId.Value && !x.Deleted)
                .Include(x => x.Loan)
                .Include(x => x.DebtStatus)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<DebtDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return debts;
        }

        public async Task<IList<DebtDto>> GetDebtsHistoryForCurrentUser()
        {
            List<DebtDto> debts = await _context.Debts
                .Where(x => x.Loan.BorrowerId == (_currentUserService.UserId.HasValue ? _currentUserService.UserId.Value : 0) && x.Deleted)
                .Include(x => x.Loan)
                .Include(x => x.DebtStatus)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<DebtDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return debts;
        }

        public async Task<IList<DebtDto>> GetCreditsHistoryForCurrentUser()
        {
            List<DebtDto> debts = await _context.Debts
                .Where(x => x.Loan.LenderId == _currentUserService.UserId.Value && x.Deleted)
                .Include(x => x.Loan)
                .Include(x => x.DebtStatus)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<DebtDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return debts;
        }

        public async Task<Result> PayDebt(DebtDto debtToUpdate)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //update debt status
                var entity = await _context.Debts
                                            .FirstOrDefaultAsync(x => x.Id == debtToUpdate.Id && !x.Deleted);

                if (entity == null)
                {
                    return Result.Failure(new List<string> { "No valid debt found" });
                }

                entity.DebtStatusId = 3;
                entity.Deleted = true;

                //update loan return date
                var loan = await _context.Loans
                                           .FirstOrDefaultAsync(x => x.Id == debtToUpdate.LoanId);

                if (loan == null)
                {
                    return Result.Failure(new List<string> { "No valid loan found" });
                }

                loan.ReturnDate = DateTime.Now;


                var wallletForCurentUser = await _context.Wallets.Where(x => x.UserId == entity.Loan.BorrowerId)
                                                                         .FirstOrDefaultAsync(x => x.CurrencyId == loan.CurrencyId);

                var walletForLender = await _context.Wallets.Where(x => x.UserId == entity.Loan.LenderId)
                                                                 .FirstOrDefaultAsync(x => x.CurrencyId == loan.CurrencyId);

                // withdraw amount from borrower and add amount to lender wallet
                if (loan.CurrencyId == wallletForCurentUser.CurrencyId)
                {
                    if (wallletForCurentUser.TotalAmount >= debtToUpdate.LoanAmount)
                    {
                        if (loan.CurrencyId == 1)
                        {
                            wallletForCurentUser.TotalAmount -= 0.97;
                        }
                        else if (loan.CurrencyId == 2)
                        {
                            wallletForCurentUser.TotalAmount -= 0.2;
                        }
                        wallletForCurentUser.TotalAmount -= debtToUpdate.LoanAmount;
                        walletForLender.TotalAmount += debtToUpdate.LoanAmount;

                    }
                    else
                    {
                        return Result.Failure(new List<string> { "Inssuficient funds for wallet in RON" });
                    }
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Result.Success("Debt was successfully payed");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                throw new Exception(e.ToString());
            }
        }

        public async Task<Result> DeferPayment(DebtDto debtToUpdate)
        {
            //update debt status
            var entity = await _context.Debts
                                        .FirstOrDefaultAsync(x => x.Id == debtToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid debt found" });
            }

            entity.DebtStatusId = 2;

            var loan = await _context.Loans
                                        .FirstOrDefaultAsync(x => x.Id == debtToUpdate.LoanId);

            if (loan == null)
            {
                return Result.Failure(new List<string> { "No valid loan found" });
            }

            if (loan.DueDate.Value.Date == DateTime.Now.Date)
            {
                loan.DueDate = DateTime.Now.AddDays(14);
            }
            else
            {
                return Result.Failure(new List<string> { "Debt cannot be deferred until due date" });
            }

            await _context.SaveChangesAsync();

            return Result.Success("Debt deferred with 14 days!");
        }

        public async Task<IList<DebtDto>> GetDebtsForUser(long id)
        {
            List<DebtDto> debts = await _context.Debts
                .Where(x => x.Loan.BorrowerId == id && !x.Deleted)
                .Include(x => x.Loan)
                .Include(x => x.DebtStatus)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<DebtDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return debts;
        }

        public async Task<Result> SendEmailReminder(long id)
        {
            try
            {
                var selectedCredit = await _context.Debts
                    .Include(x => x.Loan).ThenInclude(x => x.Borrower)
                    .Include(x => x.Loan).ThenInclude(x => x.Lender)
                    .Include(x =>x.Loan).ThenInclude(x => x.Currency)
               .FirstOrDefaultAsync(x => x.Loan.LenderId == _currentUserService.UserId.Value && !x.Deleted && x.Id == id);

                if (selectedCredit == null)
                {
                    return Result.Failure(new List<string> { "No valid credit found" });
                }


                var fromAddress = new MailAddress("speedlight.money@gmail.com", "Speedlight Money");
                var toAddress = new MailAddress(selectedCredit.Loan.Borrower.Email, selectedCredit.Loan.Borrower.GetFullName());
                const string fromPassword = "carryON09171996";
                const string subject = "[Speedlight Money] Due date Reminder";

                string body = "<div>Hi, " + selectedCredit.Loan.Borrower.FirstName + "! </div> <br/>" +
                               "<div>This is a friendly email from your lender, " + selectedCredit.Loan.Lender.GetFullName() + ", to remind you of your debt to him. Your due date is comming soon so don't forget to pay your debt!</div><br/>" +
                               "<div>Here are some details about your debt: </div>" +
                               "<div>Owed amount: " + selectedCredit.Loan.Lender.GetFullName() + "</div>" +
                                "<div>Owed amount: " + selectedCredit.Loan.Amount + " " + selectedCredit.Loan.Currency.CurrencyName + "</div>" +
                               "<div>Due date: " + selectedCredit.Loan.DueDate + "</div><br/>" +
                               "<div>You can go to the Debts tab on the application and send the money to " + selectedCredit.Loan.Lender.GetFullName() + " by clicking on the Return loan column corresponding to " + selectedCredit.Loan.Lender.GetFullName() + ". If you can't return the loan right now, don't forget you can defer the debt with 14 days from the Debts tab in the Speedlight Money App.</div>" +
                               "<div>Thank you for using our app and have a great week!</div><br/>" +
                               "<div>Speedlight Money Team</div>" +
                               "<div>Phone: 0771345665</div>" +
                               "<div>More info: speedlightmoney.com</div>";

                using var smtp = new System.Net.Mail.SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                smtp.Send(message);
                return Result.Success("Reminder was sent with success to " + selectedCredit.Loan.Borrower.GetFullName() + "!");
            }
            catch(Exception ex)
            {
                return Result.Failure(new List<string> { ex.Message });
            }

           
        }
    }

}
