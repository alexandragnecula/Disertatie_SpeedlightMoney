using System;
using AutoMapper;
using BusinessLayer.Common.Mappings;
using DataLayer.Entities;

namespace BusinessLayer.Views
{
    public class DebtDto : IMapFrom<Debt>
    {
        public long Id { get; set; }

        //LOAN
        public long LoanId { get; set; }
        public double LoanAmount { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime DueDate { get; set; }
        public string BorrowerName { get; set; }
        public string LenderName { get; set; }

        //DEBT STATUS
        public long DebtStatusId { get; set; }
        public string DebtStatusName { get; set; }

        public bool Deleted { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Debt, DebtDto>()
                .ForMember(d => d.LoanAmount,
                    opt => opt.MapFrom(s => s.Loan.Amount))
                .ForMember(d => d.BorrowDate,
                    opt => opt.MapFrom(s => s.Loan.BorrowDate))
                .ForMember(d => d.ReturnDate,
                    opt => opt.MapFrom(s => s.Loan.ReturnDate))
                .ForMember(d => d.DueDate,
                    opt => opt.MapFrom(s => s.Loan.DueDate))
                .ForMember(d => d.BorrowerName,
                    opt => opt.MapFrom(s =>
                        s.Loan.Borrower != null
                            ? s.Loan.Borrower.GetFullName()
                            : string.Empty))
                .ForMember(d => d.LenderName,
                    opt => opt.MapFrom(s =>
                        s.Loan.Lender != null
                            ? s.Loan.Lender.GetFullName()
                            : string.Empty))
                .ForMember(d => d.DebtStatusName,
                    opt => opt.MapFrom(s =>
                        s.DebtStatus != null
                            ? s.DebtStatus.DebtStatusName
                            : string.Empty));
        }
    }
}
