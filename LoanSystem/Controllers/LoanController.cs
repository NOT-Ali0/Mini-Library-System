using System.Security.Claims;
using LoanSystem.API.Common;
using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanSystem.API.Controllers
{
    public class LoanController : BaseController
    {
        private readonly ILoanService _service;

        public LoanController(ILoanService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<LoanDto>>> CreateLoan([FromBody] LoanRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(ApiResponse<LoanDto>.Fail("User not authenticated"));
            }

            var loan = await _service.CreateLoan(userId, request);
            return Ok(ApiResponse<LoanDto>.Ok(loan, "Loan created successfully"));
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<PaginatedList<LoanDto>>>> GetLoansPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var loans = await _service.GetLoansPaginated(pageNumber, pageSize);
            if (loans is null)
                return BadRequest();
            return Ok(ApiResponse<PaginatedList<LoanDto>>.Ok(loans));
        }

        [HttpGet("my-loans")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<LoanDto>>>> GetMyLoans()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(ApiResponse<List<LoanDto>>.Fail("User not authenticated"));
            }

            var loans = await _service.GetUserLoans(userId);
            return Ok(ApiResponse<List<LoanDto>>.Ok(loans));
        }
    }
}
