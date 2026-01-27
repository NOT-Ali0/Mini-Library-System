using LoanSystem.Application.DTOs;
using LoanSystem.Application.Requests;
using LoanSystem.Domain.Entities;

namespace LoanSystem.Application.Interface
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(UserRequest request);
        Task<bool> DeleteUser(int id);
        Task<List<LoanDto>> GetAllLoans( int UserId);
        Task<bool> UpdateUser(int id, UserRequest user);
        public  Task<List<UserDto>> GetAllUseres();
    }
}