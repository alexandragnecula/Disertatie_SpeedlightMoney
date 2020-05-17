using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Shared;

namespace DataLayer.Entities
{
    public class Wallet : AuditableEntity
    {
        public long Id { get; set; }
        public double TotalAmount { get; set; }
        public long UserId { get; set; }
        public long CurrencyId { get; set; }

        public ApplicationUser User { get; set; }
        public Currency Currency { get; set; }
    }
       
}
