using System.ComponentModel.DataAnnotations;
using okenovTest.Entity;
namespace okenovTest.Entity
{
    public class UserSession
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
