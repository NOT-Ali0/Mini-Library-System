using LoanSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanSystem.Infrastructure.ApplicationDbContext.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {

            builder.HasOne(b => b.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(b => b.BookId);

            builder.HasOne(u=>u.User)
                .WithMany(l=> l.Loans)
                .HasForeignKey(u => u.UserId);
                
        }

        
    }
}
