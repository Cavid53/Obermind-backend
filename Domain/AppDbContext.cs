using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain
{
    public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DatabaseFacade GetDatabase()
        {
            return base.Database;
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : BaseEntity
        {
            return Set<TEntity>();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditingEntity>())
            {
                var utcNow = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = 0;
                        entry.Entity.CreatedAt = utcNow;
                        entry.Entity.LastModifiedBy = 0;
                        entry.Entity.LastModifiedAt = utcNow;
                        break;
                    case EntityState.Modified:
                        entry.Property(e => e.CreatedBy).IsModified = false;
                        entry.Property(e => e.CreatedAt).IsModified = false;
                        entry.Entity.LastModifiedBy = 0;
                        entry.Entity.LastModifiedAt = utcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
