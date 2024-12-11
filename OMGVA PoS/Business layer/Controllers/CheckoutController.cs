using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OMGVA_PoS.Data_layer.Models;
using Stripe;

namespace OMGVA_PoS.Business_layer.Controllers
{
    [ApiController]
    [Route("checkout")]
    public class CheckoutController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost]
        public IActionResult ProcessPayment([FromBody] PaymentRequest request)
        {
            try
            {
                var paymentIntentService = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount,
                    Currency = "usd",
                    PaymentMethod = request.PaymentMethodId,
                    ConfirmationMethod = "manual",
                    Confirm = true,
                    PaymentMethodTypes = new List<string> { "card" } // Explicitly specify 'card' only
                };

                var paymentIntent = paymentIntentService.Create(options);

                if (paymentIntent.Status == "requires_action" && paymentIntent.NextAction.Type == "use_stripe_sdk")
                {
                    // Requires additional authentication (e.g., 3D Secure)
                    return Ok(new
                    {
                        requiresAction = true,
                        paymentIntentClientSecret = paymentIntent.ClientSecret
                    });
                }
                else if (paymentIntent.Status == "succeeded")
                {
                    // Payment succeeded
                    return Ok(new { success = true });
                }
                else
                {
                    // Unexpected status
                    return BadRequest(new { error = $"Invalid PaymentIntent status: {paymentIntent.Status}" });
                }
            }
            catch (StripeException e)
            {
                // Handle Stripe errors
                return BadRequest(new { error = e.StripeError.Message });
            }
            catch (System.Exception e)
            {
                // Handle other errors
                return BadRequest(new { error = e.Message });
            }
        }
    }
}