using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using okenovTest.Entity;
using okenovTest.Repositories;
using System.Security.Cryptography;
using System.Text;


namespace okenovTest.Services.impl
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserSessionRepository _userSessionRepo;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _lockoutTime = TimeSpan.FromMinutes(5);
        private readonly int _maxAttempts = 5;
        public AuthService(IUserRepository userRepo, IUserSessionRepository userSessionRepo, IMemoryCache cache)
        {
            _userRepo = userRepo;
            _userSessionRepo = userSessionRepo;
            _cache = cache;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            if (_cache.TryGetValue($"lockout_{username}", out _))
                return null;
            var user = await _userRepo.GetByUsernameAsync(username);
            if (user == null)
                return HandleFailedAttempt(username);

            var hash = ComputeHash(password);
            if (user.PasswordHash != hash)
                return HandleFailedAttempt(username);

            _cache.Remove($"attempts_{username}");
            _cache.Remove($"lockout_{username}");

            var token = Guid.NewGuid().ToString();
            await _userSessionRepo.AddAsync(new UserSession { UserId = user.Id, Token = token });
            return token;
        }
        private string? HandleFailedAttempt(string username)
        {
            var attempts = _cache.Get<int>($"attempts_{username}") + 1;
            _cache.Set($"attempts_{username}", attempts, TimeSpan.FromMinutes(5));

            if (attempts >= _maxAttempts)
            {
                _cache.Set($"lockout_{username}", true, _lockoutTime);
            }

            return null;
        }

        public async Task LogoutAsync(string token)
        {
            await _userSessionRepo.RemoveByTokenAsync(token);
        }

        private string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        public async Task<int?> ValidateTokenAsync(string token)
        {
          
            var session = await _userSessionRepo.GetByTokenAsync(token);
            if (session == null)
                return null;

            return session.UserId;
        }


    }
}
