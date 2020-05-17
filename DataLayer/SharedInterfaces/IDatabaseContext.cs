using System;
using System.Threading;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DataLayer.SharedInterfaces
{
    public interface IDatabaseContext
    {
        public DbSet<Currency> Currencies { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        public DatabaseFacade Database { get; }
    }
}
