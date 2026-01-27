using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using LoanSystem.Domain.Entities;
using LoanSystem.Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanSystemDbContext _dbContext;

        public LoanService(ILoanSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LoanDto> CreateLoan(int userId, LoanRequest request)
        {
            var loan = new Loan
            {
                UserId = userId,
                BookId = request.BookId,
                LoanDate = DateTime.UtcNow
            };

            _dbContext.Loan.Add(loan);
            await _dbContext.SaveChangesAsync();

            var book = await _dbContext.Book.FindAsync(request.BookId);

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

        public async Task<PaginatedList<LoanDto>> GetLoansPaginated(int pageNumber, int pageSize)
        {
            var query = _dbContext.Loan
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

        public async Task<List<LoanDto>> GetUserLoans(int userId)
        {
            return await _dbContext.Loan
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
