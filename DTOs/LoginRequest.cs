using Newtonsoft.Json;

namespace StringeeCallWeb.DTOs
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; } = "Nt0967311513@!!";
    }
}
