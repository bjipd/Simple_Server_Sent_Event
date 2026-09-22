using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SSE_sample.Contracts;
using System.Text.Json;

namespace SSE_sample.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SSEController : ControllerBase
    {
        [HttpGet("stream")]
        public async Task StreamEvents(CancellationToken cancellationToken)
        {
            Response.ContentType = "text/event-stream";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["Access-Control-Allow-Origin"] = "*";
            Response.Headers["X-Accel-Buffering"] = "no"; // Disable buffering for Nginx

            var symbols = new[] { "AAPL", "GOOGL", "MSFT", "AMZN", "TSLA" };
            var random = new Random();
            int eventId = 1;

            while(!cancellationToken.IsCancellationRequested)
            {
                string marketSymbol = symbols[random.Next(symbols.Length)];

                var item = new StockNewsFeed(
                    Id: eventId++,
                    Symbol: marketSymbol,
                    Headline: $"Breaking news for {marketSymbol}!",
                    Price: Math.Round((decimal)(random.NextDouble() * 100), 2),
                    Timestamp: DateTime.UtcNow
                );

                string jsonData = JsonSerializer.Serialize(item);

                await Response.WriteAsync($"id: {item.Id}\n");
                await Response.WriteAsync(($"event: stock-news\n"));
                await Response.WriteAsync($"data: {jsonData}\n\n");
                await Response.Body.FlushAsync();

                try
                {
                    await Task.Delay(1000, cancellationToken);
                }
                catch(TaskCanceledException)
                {
                    break;
                }
            }
        }
    }
}