using okenovTest.Entity;
using Microsoft.EntityFrameworkCore;


namespace okenovTest.Repositories.impl
{
    public class EfUserSessionRepository : IUserSessionRepository
    {
        private readonly AppDbContext _context;

        public EfUserSessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserSession session)
        {
            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();
        }

        public async Task<UserSession?> GetByTokenAsync(string token)
        {
            return await _context.UserSessions
                                 .FirstOrDefaultAsync(s => s.Token == token);
        }

        public async Task RemoveByTokenAsync(string token)
        {
            var session = await GetByTokenAsync(token);
            if (session != null)
            {
                _context.UserSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
    }

}
