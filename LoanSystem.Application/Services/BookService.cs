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
    public class BookService(ILoanSystemDbContext dbContext, ICacheService cacheService) : IBookService
    {
        private const string AllBooksCacheKey = "books_all";

        [Authorize("Admin")]
        public async Task<BookDto> CreateBook(BookRequest request)
        {
            var newBook = new Book()
            {
                Title = request.Title,
            };
            dbContext.Book.Add(newBook);
            await dbContext.SaveChangesAsync();
            
            cacheService.Remove(AllBooksCacheKey);
            
            return new BookDto
            {
                Id = newBook.Id,
                Title = request.Title
            };
        }
        [Authorize("Admin")]
        public async Task<bool> DeleteBook(int id)
        {
            var book = await dbContext.Book.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
                return false;
            
            book.IsDeleted = true;
            book.DeletedAt = DateTime.UtcNow;
            
            await dbContext.SaveChangesAsync();
            cacheService.Remove(AllBooksCacheKey);
            
            return true;
        }
        [Authorize("Customer")]
        public async Task<List<BookDto>> GetAllBooks()
        {
            var cached = cacheService.Get<List<BookDto>>(AllBooksCacheKey);
            if (cached != null)
            {
                return cached;
            }

            var books = await dbContext.Book
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title
                }).ToListAsync();

            cacheService.Set(AllBooksCacheKey, books, 30);
            
            return books;
        }
        [Authorize("Customer")]
        public async Task<PaginatedList<BookDto>> GetBooksPaginated(int pageNumber, int pageSize)
        {
            var query = dbContext.Book
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title
                });

            return await PaginatedList<BookDto>.CreateAsync(query, pageNumber, pageSize);
        }
        [Authorize("Admin")]
        public async Task<bool> UpdateBook(int id, BookRequest request)
        {
            var book = await dbContext.Book.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
                return false;

            book.Title = request.Title;

            dbContext.Book.Update(book);
            await dbContext.SaveChangesAsync();
            cacheService.Remove(AllBooksCacheKey);

            return true;
        }
    }
}
