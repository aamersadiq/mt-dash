using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BankDash.Model.Enitity
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public string Type { get; set; } // "Debit" or "Credit"
        [Precision(18, 2)]
        [Required]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }

}
