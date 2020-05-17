using System;
using DataLayer.Shared;

namespace DataLayer.Entities
{
    public class Debt : AuditableEntity
    {
        public long Id { get; set; }
        public long LoanId { get; set; }
        public long DebtStatusId { get; set; }

        public Loan Loan { get; set; }
        public DebtStatus DebtStatus { get; set; }
    }
}
