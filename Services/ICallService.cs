using StringeeCallWeb.DTOs;

namespace StringeeCallWeb.Services
{
    public interface ICallService
    {
        Task<GetHistoryResponse> GetHistoryCall(GetHistoryRequest request); 
    }
}
