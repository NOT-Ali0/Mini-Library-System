using LoanSystem.API.Common;
using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoanSystem.API.Controllers
{
    public class BookController(IBookService service) : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BookDto>>>> GetAllBooks()
        {
            var sw = Stopwatch.StartNew();

            

            
            var books = await service.GetAllBooks();
            sw.Stop();
            var result = ApiResponse<List<BookDto>>.Ok(books);
            return Ok( new { result , responseTimeMs = sw.ElapsedMilliseconds });
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<ApiResponse<PaginatedList<BookDto>>>> GetBooksPaginated(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var books = await service.GetBooksPaginated(pageNumber, pageSize);
            if (books is null)
                return BadRequest();
            return Ok(ApiResponse<PaginatedList<BookDto>>.Ok(books));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<BookDto>>> CreateBook([FromBody] BookRequest request)
        {
            var book = await service.CreateBook(request);
            return Ok(ApiResponse<BookDto>.Ok(book, "Book created successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> DeleteBook(int id)
        {
            var result = await service.DeleteBook(id);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("Book not found"));
            
            return Ok(ApiResponse<int>.Ok(id, "Book deleted successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> UpdateBook(int id, [FromBody] BookRequest request)
        {
            var result = await service.UpdateBook(id, request);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("Book not found"));
            
            return Ok(ApiResponse<int>.Ok(id, "Book updated successfully"));
        }
    }
}
