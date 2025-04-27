
namespace FundCoreAPI.Services.Notifications
{
    using Amazon;
    using Amazon.SimpleNotificationService;
    using Amazon.SimpleNotificationService.Model;
    using FundCoreAPI.Configuration;
    /// <summary>
    /// Service for sending notifications using AWS SNS.
    /// </summary>
    public class AwsNotificationService : INotificationService
    {
        /// <summary>
        /// AWS SNS client instance.
        /// </summary>
        private readonly AmazonSimpleNotificationServiceClient _snsClient;

        /// <summary>
        /// The ARN of the SNS topic.
        /// </summary>
        private readonly string _topicArn;

        /// <summary>
        /// Logger instance for logging information and errors.
        /// </summary>
        private readonly ILogger<AwsNotificationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsNotificationService"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration settings.</param>
        /// <param name="logger">Logger instance.</param>
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

        /// <summary>
        /// Sends an email notification using AWS SNS.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The body of the email.</param>
        /// <param name="email">The recipient's email address.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
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

        /// <summary>
        /// Sends an SMS notification using AWS SNS.
        /// </summary>
        /// <param name="message">The SMS message content.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
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
                                    StringValue = "Transactional" // Can also be "Promotional"
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

        /// <summary>
        /// Sends a notification to an SNS topic.
        /// </summary>
        /// <param name="subject">The subject of the notification.</param>
        /// <param name="message">The body of the notification.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
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
