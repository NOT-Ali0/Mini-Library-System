using LoanSystem.API.Common;
using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanSystem.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetAllUsers()
        {
            var users = await _service.GetAllUseres();
            return Ok(ApiResponse<List<UserDto>>.Ok(users));
        }

        [HttpGet("{userId}/loans")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<LoanDto>>>> GetAllUserLoans(int userId)
        {
            var loans = await _service.GetAllLoans(userId);
            return Ok(ApiResponse<List<LoanDto>>.Ok(loans));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] UserRequest request)
        {
            if (request is null)
                return BadRequest(ApiResponse<UserDto>.Fail("Invalid request"));

            var user = await _service.CreateUser(request);
            return Ok(ApiResponse<UserDto>.Ok(user, "User created successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> DeleteUser(int id)
        {
            var result = await _service.DeleteUser(id);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("User not found"));
            
            return Ok(ApiResponse<int>.Ok(id, "User deleted successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> UpdateUser(int id, [FromBody] UserRequest request)
        {
            var result = await _service.UpdateUser(id, request);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("User not found"));
            
            return Ok(ApiResponse<int>.Ok(id, "User updated successfully"));
        }
    }
}
