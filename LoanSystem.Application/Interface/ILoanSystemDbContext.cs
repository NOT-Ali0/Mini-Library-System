using LoanSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Infrastructure.ApplicationDbContext
{
    public interface ILoanSystemDbContext
    {
        DbSet<Book> Book { get; set; }
        DbSet<Loan> Loan { get; set; }
        DbSet<User> User { get; set; }
        public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
    }
}