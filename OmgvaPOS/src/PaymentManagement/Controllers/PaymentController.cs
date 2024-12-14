using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.Database.Context;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.PaymentManagement.DTOs;
using OmgvaPOS.PaymentManagement.Models;

using Stripe;
using PaymentMethod = OmgvaPOS.PaymentManagement.Enums.PaymentMethod;

namespace OMGVA_PoS.Business_layer.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly OmgvaDbContext _context;
        public PaymentController(OmgvaDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("process-card")]
        public IActionResult ProcessCardPayment([FromBody] PaymentRequest request)
        {
            var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization);
            if (businessId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }
            var business = _context.Businesses.Find(businessId);
            StripeConfiguration.ApiKey = business.StripeSecretKey;
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
        
        [HttpPost]
        [Route("process-cash")]
        public IActionResult ProcessCashPayment([FromBody] PaymentRequest request)
        {
            var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization);
            if (businessId == null)
            {
                return Unauthorized(new { error = "Unauthorized" });
            }
            var business = _context.Businesses.Find(businessId);
            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                Method = PaymentMethod.Cash,
                CustomerId = 0,
                OrderId = 0
            };
            // _context.Payments.Add(payment);
            // _context.SaveChanges();
            return Ok(new { success = true, payment });
        }
    }
}