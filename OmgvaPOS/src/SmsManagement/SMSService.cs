using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using OmgvaPOS.ItemManagement.Repositories;

namespace OmgvaPOS.SmsManagement;

public class SMSService
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly ILogger<SMSService> _logger;
    

    public SMSService(IAmazonSimpleNotificationService snsClient, ILogger<SMSService> logger)
    {
        _snsClient = snsClient;
        _logger = logger;
    }

    public async Task<string> SendSMSAsync(string phoneNumber, string message)
    {
        try
        {
            _logger.LogInformation($"Trying to send '${message}' to phoneNumber: '${phoneNumber}' ");
            var request = new PublishRequest
            {
                Message = message,
                PhoneNumber = phoneNumber // Include country code (e.g., +1234567890)
            };

            var response = await _snsClient.PublishAsync(request);
            return response.MessageId;
        }
        catch (AmazonSimpleNotificationServiceException ex)
        {
            // Handle AWS SNS specific exceptions
            _logger.LogError(ex, "Error trying to send sms");
            throw new Exception($"Error sending SMS: {ex.Message}", ex);
        }
    }
}