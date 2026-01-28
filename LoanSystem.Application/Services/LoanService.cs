using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using LoanSystem.Domain.Entities;
using LoanSystem.Infrastructure.ApplicationDbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Application.Services
{
    public class LoanService(ILoanSystemDbContext dbContext) : ILoanService
    {
        [Authorize(Roles = "Customer")]
        public async Task<LoanDto> CreateLoan(int userId, LoanRequest request)
        {
            var loan = new Loan
            {
                UserId = userId,
                BookId = request.BookId,
                LoanDate = DateTime.UtcNow
            };

            dbContext.Loan.Add(loan);
            await dbContext.SaveChangesAsync();

            var book = await dbContext.Book.FindAsync(request.BookId);

            return new LoanDto
            {
                Id = loan.Id,
                UserId = userId,
                BookId = request.BookId,
                BookTitle = book?.Title ?? string.Empty,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            };
        }
        [Authorize(Roles = "Customer")]
        public async Task<PaginatedList<LoanDto>> GetLoansPaginated(int pageNumber, int pageSize)
        {
            var query = dbContext.Loan
                .Include(l => l.Book)
                .Select(l => new LoanDto
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    BookId = l.BookId,
                    BookTitle = l.Book.Title,
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate
                });

            return await PaginatedList<LoanDto>.CreateAsync(query, pageNumber, pageSize);
        }
        [Authorize(Roles = "Customer")]
        public async Task<List<LoanDto>> GetUserLoans(int userId)
        {
            return await dbContext.Loan
                .Where(l => l.UserId == userId)
                .Include(l => l.Book)
                .Select(l => new LoanDto
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    BookId = l.BookId,
                    BookTitle = l.Book.Title,
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate
                }).ToListAsync();
        }
    }
}
