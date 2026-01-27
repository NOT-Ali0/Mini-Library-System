using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using LoanSystem.Domain.Entities;
using LoanSystem.Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly ILoanSystemDbContext _dbContext;
        private readonly ICacheService _cacheService;
        private const string AllBooksCacheKey = "books_all";

        public BookService(ILoanSystemDbContext dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<BookDto> CreateBook(BookRequest request)
        {
            var newBook = new Book()
            {
                Title = request.Title,
            };
            _dbContext.Book.Add(newBook);
            await _dbContext.SaveChangesAsync();
            
            _cacheService.Remove(AllBooksCacheKey);
            
            return new BookDto
            {
                Id = newBook.Id,
                Title = request.Title
            };
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _dbContext.Book.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
                return false;
            
            book.IsDeleted = true;
            book.DeletedAt = DateTime.UtcNow;
            
            await _dbContext.SaveChangesAsync();
            _cacheService.Remove(AllBooksCacheKey);
            
            return true;
        }

        public async Task<List<BookDto>> GetAllBooks()
        {
            var cached = _cacheService.Get<List<BookDto>>(AllBooksCacheKey);
            if (cached != null)
            {
                return cached;
            }

            var books = await _dbContext.Book
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title
                }).ToListAsync();

            _cacheService.Set(AllBooksCacheKey, books, 30);
            
            return books;
        }

        public async Task<PaginatedList<BookDto>> GetBooksPaginated(int pageNumber, int pageSize)
        {
            var query = _dbContext.Book
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title
                });

            return await PaginatedList<BookDto>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<bool> UpdateBook(int id, BookRequest request)
        {
            var book = await _dbContext.Book.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
                return false;

            book.Title = request.Title;

            _dbContext.Book.Update(book);
            await _dbContext.SaveChangesAsync();
            _cacheService.Remove(AllBooksCacheKey);

            return true;
        }
    }
}
