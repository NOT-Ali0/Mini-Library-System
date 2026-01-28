using LoanSystem.API.Common;
using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanSystem.API.Controllers
{
    public class UserController(IUserService service) : BaseController
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetAllUsers()
        {
            var users = await service.GetAllUsers();
            return Ok(ApiResponse<List<UserDto>>.Ok(users));
        }

        [HttpGet("{userId}/loans")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<LoanDto>>>> GetAllUserLoans(int userId)
        {
            var loans = await service.GetAllLoans(userId);
            return Ok(ApiResponse<List<LoanDto>>.Ok(loans));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> DeleteUser(int id)
        {
            var result = await service.DeleteUser(id);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("User not found"));
            
            return Ok(ApiResponse<int>.Ok(id, "User deleted successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> UpdateUser(int id, [FromBody] UserRequest request)
        {
            var result = await service.UpdateUser(id, request);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail("User not found"));
            
            return Ok(ApiResponse<int>.Ok(id, "User updated successfully"));
        }
    }
}
