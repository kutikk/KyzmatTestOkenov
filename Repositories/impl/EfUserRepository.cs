using Microsoft.EntityFrameworkCore;
using okenovTest.Entity;

namespace okenovTest.Repositories.impl
{
    public class EfUserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public EfUserRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
       => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
