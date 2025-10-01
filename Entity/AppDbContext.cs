using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;


namespace okenovTest.Entity
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserSession> UserSessions { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSession>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            var users = new List<User>
            {
                new User { Id = 1, Username = "kyzmat", PasswordHash = HashPassword("kyzmat123"), Balance = 8m },
                new User { Id = 2, Username = "kutman",   PasswordHash = HashPassword("password"), Balance = 8m },
                new User { Id = 3, Username = "pasha", PasswordHash = HashPassword("qwerty"), Balance = 8m }
            };

            modelBuilder.Entity<User>().HasData(users);


            base.OnModelCreating(modelBuilder);
        }
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
