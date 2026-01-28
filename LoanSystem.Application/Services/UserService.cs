using LoanSystem.Application.DTOs;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Requests;
using LoanSystem.Domain.Entities;
using LoanSystem.Infrastructure.ApplicationDbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LoanSystem.Application.Services
{
    public class UserService(ILoanSystemDbContext dbContext) : IUserService
    {
        [Authorize(Roles = "Admin")]
        public async Task<List<UserDto>> GetAllUsers()
        {
            return await dbContext.User.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToListAsync();
        }
        [Authorize(Roles ="Customer")]
        public async Task<List<LoanDto>> GetAllLoans(int UserId)
        {
            return await dbContext.Loan
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

        
        [Authorize(Roles ="Admin")]
        public async Task<bool> DeleteUser(int id)
        {
            var user = await dbContext.User.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return false;

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            
            await dbContext.SaveChangesAsync();
            return true;
        }
        [Authorize(Roles = "Admin")]
        public async Task<bool> UpdateUser(int id, UserRequest request)
        {
            var user = await dbContext.User.FirstOrDefaultAsync(_ => _.Id == id);
            if (user is null)
                return false;

            user.Name = request.Name;
            dbContext.User.Update(user);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
