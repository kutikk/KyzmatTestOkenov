using okenovTest.Entity;
using Microsoft.EntityFrameworkCore;


namespace okenovTest.Services.impl
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> MakePaymentAsync(int userId, decimal amount)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var user = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync() ?? throw new Exception("User not found");

            if (user.Balance < amount)
                throw new Exception("Insufficient funds");

            user.Balance -= amount;

            var payment = new Payment
            {
                UserId = userId,
                Amount = amount
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return payment;
        }
    }
}
