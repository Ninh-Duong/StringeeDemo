using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StringeeCallWeb.DTOs;
using StringeeCallWeb.Hubs;

namespace StringeeCallWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        private readonly IHubContext<CallHub> _hubContext;

        public CallController(IHubContext<CallHub> hubContext
                            ) 
        {
            _hubContext = hubContext;
        }

        [HttpGet("log")]
        public async Task<IActionResult> GetCallLog([FromQuery] string callId, string accessToken)
        {
            string stringeeApiUrl = "https://api.stringee.com/v1/call/log";

            //string callId = "call-vn-1-I8VPX8F4YA-1736298073484";

            //string accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJTSy4wLnBPRUJ2RnVnYnhmMlB3WmFVR0s3elZpVFRGWldDb1cxLTE3MzkyNTYzMzUiLCJpc3MiOiJTSy4wLnBPRUJ2RnVnYnhmMlB3WmFVR0s3elZpVFRGWldDb1cxIiwiZXhwIjoxNzM5MjYzNTM1LCJyZXN0X2FwaSI6dHJ1ZX0.O4NeMfgri2ZSkI5VvYP4FYr0j8WG5rFhaRB9FuWWGgE";

            string paramsRequest = $"id={callId}"; 

            if (string.IsNullOrEmpty(callId))
            {
                return BadRequest(new { success = false, message = "CallId is required" });
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-STRINGEE-AUTH", accessToken);

                var url = $"{stringeeApiUrl}?{paramsRequest}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { success = false, message = "Get call log failed", details = errorDetails });
                }

                var responseBody = await response.Content.ReadAsStringAsync();

               // var jsonResponse = JsonConvert.DeserializeObject<CallLogResponse.Root>(responseBody);

                //using var reader = new StreamReader(responseBody);

                //var body = await reader.ReadToEndAsync();

                // await _hubContext.Clients.All.SendAsync("CallLog", body);

                return Ok(responseBody);
            
            }
        }
    }
}
