namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Services.Notifications;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail(string email, string subject, string message)
        {
            var result = await _notificationService.SendEmailAsync(subject, message, email);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("send-sms")]
        public async Task<IActionResult> SendSms(string phoneNumber, string message)
        {
            var result = await _notificationService.SendSmsAsync(message, phoneNumber);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("send-to-topic")]
        public async Task<IActionResult> SendToTopic(string subject, string message)
        {
            var result = await _notificationService.SendToTopicAsync(subject, message);
            return result ? Ok() : BadRequest();
        }
    }
}
