using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using StringeeCallWeb.Services;
using StringeeCallWeb.DTOs;
using Newtonsoft.Json;

[Route("api/[controller]")]
[ApiController]
public class StringeeController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ICommonService _commonService;
    private readonly ICallService _callService;

    public StringeeController(
        IConfiguration configuration,
        ICommonService commonService,
        ICallService callService)
    {
        _configuration = configuration;
        _commonService = commonService;
        _callService = callService; 
    }

    [HttpPost("generate_token")]
    public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest generateTokenRequest)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid request data.");
        
        try
        {
            var result = await _commonService.GenerateToken(generateTokenRequest);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
        
    [HttpGet("download-recording")]
    public async Task<IActionResult> DownloadRecording([FromQuery] string callId, string accessToken)
    {
        string stringeeApiUrl = "https://api.stringee.com/v1/call/recording";

        if (string.IsNullOrEmpty(callId))
        {
            return BadRequest(new { success = false, message = "CallId is required" });
        }

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("X-STRINGEE-AUTH", accessToken);

            var url = $"{stringeeApiUrl}/{callId}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return BadRequest(new { success = false, message = "Failed to download recording", details = errorDetails });
            }

            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "audio/mpeg";
            var fileName = $"recording_{callId}.mp3";

            return File(fileBytes, contentType, fileName);
        }
    }

    [HttpGet("history-call")]
    public async Task<IActionResult> GetHistoryCall([FromQuery] GetHistoryRequest getHistoryRequest)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid request data.");

        try
        {
            var result = await _callService.GetHistoryCall(getHistoryRequest);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        string stringeeApiUrl = "https://api.stringeex.com/v1/account";

        var requestGenerateToken = new GenerateTokenRequest
        {
            UserId = "test_user",
            ActionType = 2
        };

        var tokenTMP = await _commonService.GenerateToken(requestGenerateToken);

        loginRequest.Password = "Nt0967311513@!!";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("X-STRINGEE-AUTH", tokenTMP);
            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(stringeeApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return BadRequest(new { success = false, message = "Failed login", details = errorDetails });
            }

            var responseContent = await response.Content.ReadAsStringAsync(); 
            Console.WriteLine("Response Content: " + responseContent);
            //var token = JsonConvert.DeserializeObject<string>(responseContent);
            return null;
            //return Ok(new { Token = token });
        }
    }
}
