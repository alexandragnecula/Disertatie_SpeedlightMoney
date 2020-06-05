using System;
using BusinessLayer.Common.Mappings;
using DataLayer.Entities;

namespace BusinessLayer.Views
{
    public class WalletDto : IMapFrom<Wallet>
    {
        public long Id { get; set; }
        public double TotalAmount { get; set; }
        public long UserId { get; set; }
        public long CurrencyId { get; set; }
        public bool Deleted { get; set; }
    }
}
