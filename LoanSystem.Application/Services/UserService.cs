using LoanSystem.Application.DTOs;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using LoanSystem.Domain.Entities;
using LoanSystem.Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILoanSystemDbContext _dbContext;

        public UserService(ILoanSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserDto>> GetAllUseres()
        {
            return await _dbContext.User.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToListAsync();
        }

        public async Task<List<LoanDto>> GetAllLoans(int UserId)
        {
            return await _dbContext.Loan
                .Where(l => l.UserId == UserId)
                .Include(l => l.Book)
                .Select(l => new LoanDto
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    BookId = l.BookId,
                    BookTitle = l.Book.Title,
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate
                }).ToListAsync();
        }

        public async Task<UserDto> CreateUser(UserRequest request)
        {
            var newUser = new User()
            {
                Name = request.Name.Trim(),
            };

            _dbContext.User.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return new UserDto
            {
                Id = newUser.Id,
                Name = request.Name.Trim()
            };
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return false;

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUser(int id, UserRequest request)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(_ => _.Id == id);
            if (user is null)
                return false;

            user.Name = request.Name;
            _dbContext.User.Update(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
