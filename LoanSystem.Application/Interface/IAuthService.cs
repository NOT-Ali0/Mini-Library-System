using LoanSystem.Application.DTOs.Auth;

namespace LoanSystem.Application.Interface
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> Register(RegisterDto request);
        Task<TokenResponseDto?> Login(LoginDto request);
    }
}
