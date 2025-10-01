using Microsoft.AspNetCore.Mvc;
using okenovTest.Services;

namespace okenovTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IAuthService _authService;

        public PaymentController(IPaymentService paymentService, IAuthService authService)
        {
            _paymentService = paymentService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment([FromHeader(Name = "Authorization")] string token)
        {
            var userId = await _authService.ValidateTokenAsync(token);
            if (userId == null)
                return Unauthorized("Invalid token");


            try
            {
                var payment = await _paymentService.MakePaymentAsync(userId.Value, 1.1m);
                return Ok(new { payment.Id, payment.Amount, payment.CreatedAt });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
