using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoanSystem.Application.DTOs.Auth;
using LoanSystem.Application.Interface;
using LoanSystem.Domain.Entities;
using LoanSystem.Domain.Enums;
using LoanSystem.Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LoanSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILoanSystemDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(ILoanSystemDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<TokenResponseDto?> Register(RegisterDto request)
        {
            var existingUser = await _dbContext.User
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return null;
            }

            var role = Enum.TryParse<UserRole>(request.Role, true, out var parsedRole) 
                ? parsedRole 
                : UserRole.Customer;

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = role
            };

            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();

            return new TokenResponseDto
            {
                Token = GenerateToken(user)
            };
        }

        public async Task<TokenResponseDto?> Login(LoginDto request)
        {
            var user = await _dbContext.User
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            return new TokenResponseDto
            {
                Token = GenerateToken(user)
            };
        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "SuperSecretKeyForJwtAuthentication123!"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "LoanSystem",
                audience: _configuration["Jwt:Audience"] ?? "LoanSystem",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
