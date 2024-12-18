using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.BusinessManagement.Services;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.PaymentManagement.DTOs;
using OmgvaPOS.PaymentManagement.Mappers;
using OmgvaPOS.PaymentManagement.Models;
using OmgvaPOS.PaymentManagement.Services;
using Stripe;
using PaymentMethod = OmgvaPOS.PaymentManagement.Enums.PaymentMethod;

namespace OMGVA_PoS.Business_layer.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IBusinessService _businessService;
        public PaymentController(IPaymentService paymentService, IBusinessService businessService)
        {
            _paymentService = paymentService;
            _businessService = businessService;
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<PaymentDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPayments()
        {
            var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization);
            if (businessId == null)
                return Forbid();

            var results = _paymentService.GetPayments();
            return Ok(results);
        }
        
        [HttpGet("{orderId}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<PaymentDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPayment([FromRoute] long orderId)
        {
            var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization);
            if (businessId == null)
                return Forbid();
            
            var results = _paymentService.GetPayment(orderId);
            if (results.Count == 0)
                return NotFound(new { Message = "No payments found with provided orderId" });
            
            return Ok(results);
        }

        [HttpPost("process-card")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<PaymentDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ProcessCardPayment([FromBody] PaymentRequest request)
        {
            var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization);
            if (businessId == null)
                return Forbid();
            
            var business = _businessService.GetBusiness(businessId);
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
                    var payment = new Payment
                    {
                        Id = paymentIntent.Id,
                        Method = PaymentMethod.Card,
                        CustomerId = request.CustomerId,
                        OrderId = request.OrderId,
                        Amount = request.Amount
                    };
                    _paymentService.CreatePayment(payment.ToPaymentDTO());
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
        
        [HttpPost("process-cash")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<PaymentDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ProcessCashPayment([FromBody] PaymentRequest request)
        {
            var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization);
            if (businessId == null)
                return Forbid();
            
            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                Method = PaymentMethod.Cash,
                CustomerId = request.CustomerId,
                OrderId = request.OrderId,
                Amount = request.Amount
            };
            
            _paymentService.CreatePayment(payment.ToPaymentDTO());
            return Ok(new { success = true, payment });
        }
        
        [HttpPost("process-giftcard")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<PaymentDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ProcessGiftcardPayment([FromBody] PaymentRequest request)
        {
            var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization);
            if (businessId == null)
                return Forbid();
            
            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                Method = PaymentMethod.Giftcard,
                CustomerId = request.CustomerId,
                OrderId = request.OrderId,
                Amount = request.Amount,
                GiftCardPaymentId = request.GiftCardPaymentId
            };
            
            _paymentService.CreatePayment(payment.ToPaymentDTO());
            return Ok(new { success = true, payment });
        }
    }
}