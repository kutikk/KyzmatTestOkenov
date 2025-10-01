using okenovTest.Entity;

namespace okenovTest.Repositories
{
    public interface IUserSessionRepository
    {
        Task AddAsync(UserSession session);                  
        Task<UserSession?> GetByTokenAsync(string token);   
        Task RemoveByTokenAsync(string token);
    }
}
