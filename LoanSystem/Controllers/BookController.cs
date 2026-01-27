using LoanSystem.API.Common;
using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanSystem.API.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BookDto>>>> GetAllBooks()
        {
            var books = await _service.GetAllBooks();
            return Ok(ApiResponse<List<BookDto>>.Ok(books));
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<ApiResponse<PaginatedList<BookDto>>>> GetBooksPaginated(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var books = await _service.GetBooksPaginated(pageNumber, pageSize);
            if (books is null)
                return BadRequest();
            return Ok(ApiResponse<PaginatedList<BookDto>>.Ok(books));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<BookDto>>> CreateBook([FromBody] BookRequest request)
        {
            var book = await _service.CreateBook(request);
            return Ok(ApiResponse<BookDto>.Ok(book, "Book created successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> DeleteBook(int id)
        {
            var result = await _service.DeleteBook(id);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("Book not found"));
            
            return Ok(ApiResponse<int>.Ok(id, "Book deleted successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> UpdateBook(int id, [FromBody] BookRequest request)
        {
            var result = await _service.UpdateBook(id, request);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("Book not found"));
            
            return Ok(ApiResponse<int>.Ok(id, "Book updated successfully"));
        }
    }
}
