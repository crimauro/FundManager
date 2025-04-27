namespace FundCoreAPI.Configuration
{
    /// <summary>
    /// Represents the AWS settings configuration.
    /// </summary>
    public class AwsSettings
    {
        /// <summary>
        /// Gets or sets the AWS region.
        /// </summary>
        public required string Region { get; set; }

        /// <summary>
        /// Gets or sets the AWS access key.
        /// </summary>
        public required string AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the AWS secret key.
        /// </summary>
        public required string SecretKey { get; set; }
    }

}
