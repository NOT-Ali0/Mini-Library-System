using LoanSystem.Application.DTOs;
using LoanSystem.Application.DTOs.Common;
using LoanSystem.Application.Requests;

namespace LoanSystem.Application.Interface
{
    public interface ILoanService
    {
        Task<LoanDto> CreateLoan(int userId, LoanRequest request);
        Task<PaginatedList<LoanDto>> GetLoansPaginated(int pageNumber, int pageSize);
        Task<List<LoanDto>> GetUserLoans(int userId);
    }
}
