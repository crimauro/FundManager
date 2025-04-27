namespace FundCoreAPI.Configuration
{
    /// <summary>
    /// Configuration settings for AWS notifications.
    /// </summary>
    public class AwsNotificationSettings
    {
        /// <summary>
        /// The AWS region where the notifications will be sent. Default is "us-east-1".
        /// </summary>
        public string Region { get; set; } = "us-east-1";

        /// <summary>
        /// The AWS access key used for authentication.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// The AWS secret key used for authentication.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// The Amazon Resource Name (ARN) of the topic to which notifications will be published.
        /// </summary>
        public string TopicArn { get; set; }
    }
}
