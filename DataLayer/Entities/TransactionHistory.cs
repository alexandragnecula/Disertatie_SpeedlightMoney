using System;
using DataLayer.Shared;

namespace DataLayer.Entities
{
    public class TransactionHistory : AuditableEntity
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public long SenderId { get; set; }
        public long BeneficiarId { get; set; }
        public long CurrencyId { get; set; }

        public ApplicationUser Sender { get; set; }
        public ApplicationUser Beneficiar { get; set; }
        public Currency Currency { get; set; }
    }
}
