using StringeeCallWeb.DTOs;

namespace StringeeCallWeb.Services
{
    public interface ICommonService
    {
        Task<string> GenerateToken(GenerateTokenRequest generateTokenRequest);
    }
}
