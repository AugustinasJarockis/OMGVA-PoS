using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace OMGVA_PoS.Business_layer.Controllers;

[ApiController]
[Route("stripekeys")]
public class StripeKeysController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public StripeKeysController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("publishableKey")]
    public IActionResult GetPublishableKey()
    {
        var key = _configuration["Stripe:PublishableKey"];
        return Ok(new { publishableKey = key });
    }
}