using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Requests;

namespace LoanSystem.Application.Interface
{
    public interface IBookService
    {
        Task<BookDto> CreateBook(BookRequest Book);
        Task<bool> DeleteBook(int id);
        Task<List<BookDto>> GetAllBooks();
        Task<PaginatedList<BookDto>> GetBooksPaginated(int pageNumber, int pageSize);
        Task<bool> UpdateBook(int id, BookRequest Book);
    }
}
