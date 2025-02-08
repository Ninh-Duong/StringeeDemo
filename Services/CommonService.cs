using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StringeeCallWeb.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StringeeCallWeb.Services
{
    public class CommonService : ICommonService
    {
        private readonly ILogger<StringeeController> _logger;
        private readonly IConfiguration _configuration;

        public CommonService(
            ILogger<StringeeController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GenerateToken(GenerateTokenRequest generateTokenRequest)
        {
            try
            {
                var apiKeySid = _configuration["Stringee:ApiKeySid"];
                var apiKeySecret = _configuration["Stringee:ApiKeySecret"];

                if (string.IsNullOrEmpty(apiKeySid) || string.IsNullOrEmpty(apiKeySecret) || string.IsNullOrEmpty(generateTokenRequest.UserId))
                {
                    throw new InvalidOperationException("APIKeySid or APIKeySecret null ");
                }

                var now = DateTime.UtcNow;
                var exp = now.AddHours(2);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, $"{apiKeySid}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Iss, apiKeySid),
                    new Claim(JwtRegisteredClaimNames.Exp, ((DateTimeOffset)exp).ToUnixTimeSeconds().ToString()),
                };
                switch (generateTokenRequest.ActionType)
                {
                    case 1:
                        claims.Add(new Claim("userId", generateTokenRequest.UserId));
                        break;
                    case 2:
                        claims.Add(new Claim("rest_api", "true", ClaimValueTypes.Boolean));
                        break;
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiKeySecret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: apiKeySid,
                    claims: claims,
                    expires: exp,
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while generating the token.");
                return null;
            }
        }
    }
}
