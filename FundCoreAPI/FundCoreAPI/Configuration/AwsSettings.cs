namespace FundCoreAPI.Configuration
{
    public class AwsSettings
    {
        public required string Region { get; set; }
        public required string AccessKey { get; set; }
        public required string SecretKey { get; set; }
    }

}
