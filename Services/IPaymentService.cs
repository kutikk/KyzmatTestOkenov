using okenovTest.Entity;

namespace okenovTest.Services
{
    public interface IPaymentService
    {
        Task<Payment> MakePaymentAsync(int userId, decimal amount);
    }
}
