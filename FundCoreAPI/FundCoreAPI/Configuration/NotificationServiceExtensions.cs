using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using FundCoreAPI.Services.Notifications;
using System.Text.Json;

namespace FundCoreAPI.Configuration
{
    public static class NotificationServiceExtensions
    {
        public static IServiceCollection AddAwsNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            // Cargar secretos desde AWS Secrets Manager
            var secretsManagerClient = new AmazonSecretsManagerClient();
            var secretName = "FundManagerAppSecrets"; // Nombre del secreto
            var secretValueResponse = secretsManagerClient.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = secretName
            }).Result;

            if (secretValueResponse.SecretString != null)
            {
                // Deserializar los secretos y mapearlos a AwsNotificationSettings
                var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(secretValueResponse.SecretString);
                var awsNotificationSettings = new AwsNotificationSettings
                {
                    Region = secrets["AwsNotificationSettings:Region"],
                    AccessKey = secrets["AwsNotificationSettings:AccessKey"],
                    SecretKey = secrets["AwsNotificationSettings:SecretKey"],
                    TopicArn = secrets["AwsNotificationSettings:TopicArn"]
                };

                // Registrar AwsNotificationSettings como un servicio singleton
                services.AddSingleton(awsNotificationSettings);
            }

            // Registrar el servicio de notificaciones
            services.AddSingleton<INotificationService, AwsNotificationService>();
            return services;
        }
    }
}
