using System;
using System.Collections.Generic;
using DataLayer.Shared;

namespace DataLayer.Entities
{
    public class LoanStatus : AuditableEntity
    {
        public LoanStatus()
        {
            Loans = new HashSet<Loan>();
        }

        public long Id { get; set; }
        public string LoanStatusName { get; set; }

        public ICollection<Loan> Loans { get; private set; }
    }
}
