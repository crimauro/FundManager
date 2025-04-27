namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Services.Notifications;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for handling notification-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="notificationService">The notification service to handle notifications.</param>
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Sends an email notification.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The body of the email.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail(string email, string subject, string message)
        {
            var result = await _notificationService.SendEmailAsync(subject, message, email);
            return result ? Ok() : BadRequest();
        }

        /// <summary>
        /// Sends an SMS notification.
        /// </summary>
        /// <param name="phoneNumber">The recipient's phone number.</param>
        /// <param name="message">The body of the SMS.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("send-sms")]
        public async Task<IActionResult> SendSms(string phoneNumber, string message)
        {
            var result = await _notificationService.SendSmsAsync(message, phoneNumber);
            return result ? Ok() : BadRequest();
        }

        /// <summary>
        /// Sends a notification to a topic.
        /// </summary>
        /// <param name="subject">The subject of the notification.</param>
        /// <param name="message">The body of the notification.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("send-to-topic")]
        public async Task<IActionResult> SendToTopic(string subject, string message)
        {
            var result = await _notificationService.SendToTopicAsync(subject, message);
            return result ? Ok() : BadRequest();
        }
    }
}
