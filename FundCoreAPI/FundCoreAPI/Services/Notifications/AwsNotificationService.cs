
namespace FundCoreAPI.Services.Notifications
{
    using Amazon;
    using Amazon.SimpleNotificationService;
    using Amazon.SimpleNotificationService.Model;
    using FundCoreAPI.Configuration;
    public class AwsNotificationService : INotificationService
    {
        private readonly AmazonSimpleNotificationServiceClient _snsClient;
        private readonly string _topicArn;
        private readonly ILogger<AwsNotificationService> _logger;

        public AwsNotificationService(IConfiguration configuration, ILogger<AwsNotificationService> logger)
        {
            var settings = configuration.GetSection("AwsNotificationSettings").Get<AwsNotificationSettings>();

            _logger = logger;
            _topicArn = settings.TopicArn;

            var region = RegionEndpoint.GetBySystemName(settings.Region);
            _snsClient = new AmazonSimpleNotificationServiceClient(
                settings.AccessKey,
                settings.SecretKey,
                region);
        }

        public async Task<bool> SendEmailAsync(string subject, string message, string email)
        {
            try
            {
                var request = new PublishRequest
                {
                    Subject = subject,
                    Message = message,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        {
                            "email", new MessageAttributeValue
                            {
                                DataType = "String",
                                StringValue = email
                            }
                        }
                    },
                    TargetArn = _topicArn
                };

                var response = await _snsClient.PublishAsync(request);
                _logger.LogInformation($"Email notification sent: {response.MessageId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email notification");
                return false;
            }
        }

        public async Task<bool> SendSmsAsync(string message, string phoneNumber)
        {
            try
            {
                var request = new PublishRequest
                {
                    Message = message,
                    PhoneNumber = phoneNumber,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        {
                            "AWS.SNS.SMS.SMSType", new MessageAttributeValue
                            {
                                DataType = "String",
                                StringValue = "Transactional" // Puede ser "Promotional" también
                            }
                        }
                    }
                };

                var response = await _snsClient.PublishAsync(request);
                _logger.LogInformation($"SMS notification sent: {response.MessageId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS notification");
                return false;
            }
        }

        public async Task<bool> SendToTopicAsync(string subject, string message)
        {
            try
            {
                var request = new PublishRequest
                {
                    Subject = subject,
                    Message = message,
                    TopicArn = _topicArn
                };

                var response = await _snsClient.PublishAsync(request);
                _logger.LogInformation($"Topic notification sent: {response.MessageId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending topic notification");
                return false;
            }
        }
    }
}
