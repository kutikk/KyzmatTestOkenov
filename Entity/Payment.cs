using System.ComponentModel.DataAnnotations;
using okenovTest.Entity;

namespace okenovTest.Entity
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; } = 1.1m;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
