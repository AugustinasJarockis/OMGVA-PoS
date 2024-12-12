using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using OMGVA_PoS.Data_layer.Models;
using OMGVA_PoS.Helper_modules.Utilities;
using Stripe;

namespace OMGVA_PoS.Business_layer.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost]
        [Route("process_card_payment")]
        public IActionResult ProcessCardPayment([FromBody] PaymentRequest request)
        {
            // JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);
            // if (token == null)
            // {
            //     return Forbid();
            // }
            
            try
            {
                var paymentIntentService = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount,
                    Currency = "eur",
                    PaymentMethod = request.PaymentMethodId,
                    ConfirmationMethod = "manual",
                    Confirm = true,
                    PaymentMethodTypes = new List<string> { "card" }
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
                    return Ok(new
                    {
                        success = true,
                        paymentIntent = new
                        {
                            id = paymentIntent.Id,
                            amount = paymentIntent.Amount,
                            currency = paymentIntent.Currency,
                            status = paymentIntent.Status,
                            description = paymentIntent.Description,
                            metadata = paymentIntent.Metadata,
                            created = paymentIntent.Created,
                            // Add other fields as needed
                        }
                    });
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