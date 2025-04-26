namespace FundCoreAPI.Configuration
{
    public class AwsNotificationSettings
    {
        public string Region { get; set; } = "us-east-1";
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string TopicArn { get; set; }
    }
}
