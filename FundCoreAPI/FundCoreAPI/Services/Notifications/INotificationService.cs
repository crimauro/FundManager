namespace FundCoreAPI.Services.Notifications
{
    /// <summary>
    /// Interface for notification services, providing methods to send emails, SMS, and topic-based messages.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The body of the email.</param>
        /// <param name="email">The recipient's email address.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        Task<bool> SendEmailAsync(string subject, string message, string email);

        /// <summary>
        /// Sends an SMS asynchronously.
        /// </summary>
        /// <param name="message">The content of the SMS.</param>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        Task<bool> SendSmsAsync(string message, string phoneNumber);

        /// <summary>
        /// Sends a message to a topic asynchronously.
        /// </summary>
        /// <param name="subject">The subject of the message.</param>
        /// <param name="message">The body of the message.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        Task<bool> SendToTopicAsync(string subject, string message);
    }
}
