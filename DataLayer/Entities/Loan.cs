using System;
using System.Collections.Generic;
using DataLayer.Shared;

namespace DataLayer.Entities
{
    public class Loan : AuditableEntity
    {
        public Loan()
        {
            Debts = new HashSet<Debt>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? DueDate { get; set; }
        public long BorrowerId { get; set; }
        public long LenderId { get; set; }
        public long CurrencyId { get; set; }
        public long LoanStatusId { get; set; }
        public long TermId { get; set; }

        public ApplicationUser Borrower { get; set; }
        public ApplicationUser Lender { get; set; }
        public Currency Currency { get; set; }
        public LoanStatus LoanStatus { get; set; }
        public Term Term { get; set; }

        public ICollection<Debt> Debts { get; private set; }
    }
}
