using System;
using System.Collections.Generic;
using DataLayer.Shared;

namespace DataLayer.Entities
{
    public class Term : AuditableEntity
    {
        public Term()
        {
            Loans = new HashSet<Loan>();
        }

        public long Id { get; set; }
        public string TermName { get; set; }
        public int PeriodInDays { get; set; }

        public ICollection<Loan> Loans { get; private set; }
    }
}
