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
            // Load secrets from AWS Secrets Manager  
            var secretsManagerClient = new AmazonSecretsManagerClient();
            var secretName = "FundManagerAppSecrets"; // Secret name  
            var secretValueResponse = secretsManagerClient.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = secretName
            }).Result;

            if (secretValueResponse.SecretString != null)
            {
                // Deserialize secrets and map them to AwsNotificationSettings  
                var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(secretValueResponse.SecretString);
                var awsNotificationSettings = new AwsNotificationSettings
                {
                    /// <summary>  
                    /// The AWS region for the notification service.  
                    /// </summary>  
                    Region = secrets["AwsNotificationSettings:Region"],

                    /// <summary>  
                    /// The AWS access key for authentication.  
                    /// </summary>  
                    AccessKey = secrets["AwsNotificationSettings:AccessKey"],

                    /// <summary>  
                    /// The AWS secret key for authentication.  
                    /// </summary>  
                    SecretKey = secrets["AwsNotificationSettings:SecretKey"],

                    /// <summary>  
                    /// The ARN of the AWS SNS topic.  
                    /// </summary>  
                    TopicArn = secrets["AwsNotificationSettings:TopicArn"]
                };

                // Register AwsNotificationSettings as a singleton service  
                services.AddSingleton(awsNotificationSettings);
            }

            // Register the notification service  
            services.AddSingleton<INotificationService, AwsNotificationService>();
            return services;
        }
    }
}
