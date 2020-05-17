using System;
using System.Collections.Generic;
using DataLayer.Shared;

namespace DataLayer.Entities
{
    public class DebtStatus : AuditableEntity
    {
        public DebtStatus()
        {
            Debts = new HashSet<Debt>();
        }

        public long Id { get; set; }
        public string DebtStatusName { get; set; }

        public ICollection<Debt> Debts { get; private set; }
    }
}
