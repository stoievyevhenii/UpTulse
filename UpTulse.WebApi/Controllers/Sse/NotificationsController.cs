using Microsoft.AspNetCore.Mvc;

using UpTulse.Application.Managers;

namespace UpTulse.WebApi.Controllers.Sse
{
    public class NotificationsController : SseController
    {
        private readonly INotificationSseManager _notificationSseManager;

        public NotificationsController(INotificationSseManager notificationSseManager)
        {
            _notificationSseManager = notificationSseManager;
        }

        [HttpGet("stream")]
        public async Task Get(CancellationToken ct)
        {
            Response.Headers.Append("Content-Type", "text/event-stream");
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");

            await foreach (var message in _notificationSseManager.Subscribe(ct))
            {
                var json = System.Text.Json.JsonSerializer.Serialize(message);

                await Response.WriteAsync($"data: {json}\n\n", ct);
                await Response.Body.FlushAsync(ct);
            }
        }
    }
}