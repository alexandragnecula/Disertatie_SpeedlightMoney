using System;
using AutoMapper;
using BusinessLayer.Common.Mappings;
using DataLayer.Entities;

namespace BusinessLayer.Views
{
    public class LoanDto : IMapFrom<Loan>
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime DueDate { get; set; }

        //BORROWER
        public long BorrowerId { get; set; }
        public string BorrowerName { get; set; }
        //LENDER
        public long LenderId { get; set; }
        public string LenderName { get; set; }

        //CURRENCY
        public long CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        //LOAN STATUS
        public long LoanStatusId { get; set; }
        public string LoanStatusName { get; set; }

        //TERM 
        public long TermId { get; set; }
        public string TermName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Loan, LoanDto>()
                 .ForMember(d => d.BorrowerName,
                    opt => opt.MapFrom(s =>
                        s.Borrower != null
                            ? s.Borrower.GetFullName()
                            : string.Empty))
                  .ForMember(d => d.LenderName,
                    opt => opt.MapFrom(s =>
                        s.Lender != null
                            ? s.Lender.GetFullName()
                            : string.Empty))
                  .ForMember(d => d.CurrencyName,
                    opt => opt.MapFrom(s =>
                        s.Currency != null
                            ? s.Currency.CurrencyName
                            : string.Empty))
                  .ForMember(d => d.LoanStatusName,
                    opt => opt.MapFrom(s =>
                        s.LoanStatus != null
                            ? s.LoanStatus.LoanStatusName
                            : string.Empty))
                  .ForMember(d => d.TermName,
                    opt => opt.MapFrom(s =>
                        s.Term != null
                            ? s.Term.TermName
                            : string.Empty));
        }
    }
}
