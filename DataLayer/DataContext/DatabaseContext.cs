using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DataLayer.Entities;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using DataLayer.Shared;
using DataLayer.SharedInterfaces;

namespace DataLayer.DataContext
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        private readonly ICurrentUserService _currentUserService;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<LoanStatus> LoanStatuses { get; set; }
        public virtual DbSet<Term> Terms { get; set; }
        public virtual DbSet<Debt> Debts { get; set; }
        public virtual DbSet<DebtStatus> DebtStatuses { get; set; }
        public virtual DbSet<TransactionHistory> TransactionsHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Model.GetEntityTypes().ToList().ForEach(entityType => entityType.GetForeignKeys()
              .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
              .ToList()
              .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict));

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());         
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedOn = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        if (entry.Entity.Deleted)
                        {
                            entry.Entity.DeletedBy = _currentUserService.UserId;
                            entry.Entity.DeletedOn = DateTime.Now;
                        }
                        else
                        {
                            entry.Entity.LastModifiedBy = _currentUserService.UserId;
                            entry.Entity.LastModifiedOn = DateTime.Now;
                        }

                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
