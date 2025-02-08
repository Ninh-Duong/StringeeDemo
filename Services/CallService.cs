using Newtonsoft.Json.Linq;
using StringeeCallWeb.DTOs;

namespace StringeeCallWeb.Services
{
    public class CallService : ICallService
    {
        public async Task<GetHistoryResponse> GetHistoryCall(GetHistoryRequest request)
        {
            string stringeeApiUrl = "https://api.stringeex.com/v1/call/history";

            if (string.IsNullOrEmpty(request.CallId))
            {
                throw new InvalidOperationException("CallId null ");
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-STRINGEE-AUTH", request.Token);

                var url = $"{stringeeApiUrl}?{request.CallId}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return null;
                    //return BadRequest(new { success = false, message = "Failed to download recording", details = errorDetails });
                }
            }
            return null;
        }
    }
}
