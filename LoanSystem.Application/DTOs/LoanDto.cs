using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanSystem.Application.DTOs
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
