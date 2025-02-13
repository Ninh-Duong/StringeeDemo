using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StringeeCallWeb.Hubs;

namespace StringeeCallWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHook : ControllerBase
    {
        private readonly IHubContext<CallHub> _hubContext;

        public WebHook(IHubContext<CallHub> hubContext)
        {
            _hubContext = hubContext;
        }    

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                Console.WriteLine("Webhook received:");
                //Console.WriteLine(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(body), Formatting.Indented));

                await _hubContext.Clients.All.SendAsync("ReceiveCallData", body);

                return Ok(new { message = "Webhook received successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing webhook: " + ex.Message);
                return BadRequest(new { error = "Failed to process webhook" });
            }

            //set NODE_TLS_REJECT_UNAUTHORIZED=0 && npx smee-client --url https://smee.io/PxTu9vLQk2Mue9lP --target http://localhost:5140/api/webhook
        }
    }
}
