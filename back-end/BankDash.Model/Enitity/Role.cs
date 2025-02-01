using System.ComponentModel.DataAnnotations;

namespace BankDash.Model.Enitity
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
