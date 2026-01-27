using LoanSystem.Domain.Entities;
using LoanSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Infrastructure.ApplicationDbContext
{
    public class LoanSystemDbContext : DbContext, ILoanSystemDbContext
    {
        public LoanSystemDbContext(DbContextOptions<LoanSystemDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Loan> Loan { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleAuditableEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void HandleAuditableEntities()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable && 
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var auditable = (IAuditable)entry.Entity;
                auditable.UpdatedAt = DateTime.UtcNow;
                
                if (entry.State == EntityState.Modified)
                {
                    var changes = new List<string>();
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.IsModified && prop.Metadata.Name != nameof(IAuditable.Changes) 
                            && prop.Metadata.Name != nameof(IAuditable.UpdatedAt))
                        {
                            changes.Add($"{prop.Metadata.Name}: {prop.OriginalValue} -> {prop.CurrentValue}");
                        }
                    }
                    if (changes.Any())
                    {
                        auditable.Changes = string.Join("; ", changes);
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanSystemDbContext).Assembly);
            
            modelBuilder.Entity<User>().HasQueryFilter(u => u.IsDeleted != true);
            modelBuilder.Entity<Book>().HasQueryFilter(b => b.IsDeleted != true);
        }
    }
}
