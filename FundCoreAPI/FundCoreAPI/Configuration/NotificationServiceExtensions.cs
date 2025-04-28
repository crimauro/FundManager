using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using FundCoreAPI.Services.Notifications;
using System.Text.Json;

namespace FundCoreAPI.Configuration
{
    /// <summary>  
    /// Provides extension methods for configuring the AWS Notification Service.  
    /// </summary>  
    public static class NotificationServiceExtensions
    {
        /// <summary>  
        /// Adds the AWS Notification Service to the service collection.  
        /// </summary>  
        /// <param name="services">The service collection to which the service will be added.</param>  
        /// <param name="configuration">The application configuration.</param>  
        /// <returns>The updated service collection.</returns>  
        public static IServiceCollection AddAwsNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            /// <summary>
            /// Load credentials.
            /// </summary>
            var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");
            var awsTopicArn = Environment.GetEnvironmentVariable("AWS_TOPIC_ARN");

            var awsNotificationSettings = new AwsNotificationSettings
            {
                /// <summary>  
                /// The AWS region for the notification service.  
                /// </summary>  
                Region = awsRegion,

                /// <summary>  
                /// The AWS access key for authentication.  
                /// </summary>  
                AccessKey = awsAccessKey,

                /// <summary>  
                /// The AWS secret key for authentication.  
                /// </summary>  
                SecretKey = awsSecretKey,

                /// <summary>  
                /// The ARN of the AWS SNS topic.  
                /// </summary>  
                TopicArn = awsTopicArn
            };

            // Register AwsNotificationSettings as a singleton service  
            services.AddSingleton(awsNotificationSettings);

            // Register the notification service  
            services.AddSingleton<INotificationService, AwsNotificationService>();
            return services;
        }
    }
}
