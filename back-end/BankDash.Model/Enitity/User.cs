using System.ComponentModel.DataAnnotations;

namespace BankDash.Model.Enitity
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
