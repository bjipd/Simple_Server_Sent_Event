using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SSE_sample.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SSEController : ControllerBase
    {
        [HttpGet("stream")]
        public async Task StreamEvents()
        {
            Response.ContentType = "text/event-stream";

            for (int p = 0; p < 10; p++)
            {
                await Response.WriteAsync($"Data: Event {p+1} \n\n");
                await Response.Body.FlushAsync(); 
                await Task.Delay(1000);
            }
        }
    }
}