using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Interfaces;

namespace LoanSystem.Domain.Entities
{
    public class User : ISoftDeletable, IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Customer;
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

        public bool? IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? Changes { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
