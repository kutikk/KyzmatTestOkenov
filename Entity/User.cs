using System.ComponentModel.DataAnnotations;
using okenovTest.Entity;
namespace okenovTest.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; 
        public decimal Balance { get; set; } = 8.0m;    
        public ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
