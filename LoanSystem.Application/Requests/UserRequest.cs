using System.ComponentModel.DataAnnotations;

namespace LoanSystem.Application.Requests
{
    public class UserRequest
    {
        [MaxLength(20)]
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
