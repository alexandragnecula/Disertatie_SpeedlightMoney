using System;
namespace BusinessLayer.Views
{
    public class WalletDto
    {
        public long Id { get; set; }
        public double TotalAmount { get; set; }
        public long UserId { get; set; }
        public long CurrencyId { get; set; }
        public bool Deleted { get; set; }
    }
}
