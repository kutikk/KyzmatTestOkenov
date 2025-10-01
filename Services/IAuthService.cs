namespace okenovTest.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string username, string password);
        Task LogoutAsync(string token);
        Task <int?> ValidateTokenAsync(string token);
    }
}
