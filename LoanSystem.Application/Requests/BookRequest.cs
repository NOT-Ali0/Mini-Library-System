using System.ComponentModel.DataAnnotations;

namespace LoanSystem.Application.Requests
{
    public class BookRequest
    {
        [MaxLength(250)]
        [Required]
        public string Title { get; set; } = string.Empty;
    }
}
