namespace FundCoreAPI.Services.Notifications
{
    public interface INotificationService
    {
        Task<bool> SendEmailAsync(string subject, string message, string email);
        Task<bool> SendSmsAsync(string message, string phoneNumber);
        Task<bool> SendToTopicAsync(string subject, string message);
    }
}
