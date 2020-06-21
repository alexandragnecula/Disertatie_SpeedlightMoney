using System;
using System.Collections.Generic;
using DataLayer.Shared;

namespace DataLayer.Entities
{
    public class Currency :AuditableEntity
    {
        public Currency()
        {
            Wallets = new HashSet<Wallet>();
            Loans = new HashSet<Loan>();
            TransactionsHistory = new HashSet<TransactionHistory>();
        }

        public long Id { get; set; }
        public string CurrencyName { get; set; }

        public ICollection<Wallet> Wallets { get; private set; }
        public ICollection<Loan> Loans { get; private set; }
        public ICollection<TransactionHistory> TransactionsHistory { get; private set; }
    }
}
