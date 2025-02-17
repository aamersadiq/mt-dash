using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BankDash.Model.Enitity
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [Precision(18, 2)]
        [Required]
        public decimal Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }

}
