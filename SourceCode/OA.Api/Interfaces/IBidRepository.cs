using OA.Api.Requests;
using OA.Api.Responses;


namespace OA.Api.Interfaces
{
    public interface IBidRepository
    {
        Task<bool> Create(CreateBidRequest model);
        Task<BidResponse[]> GetAllBid(int itemId);
    }
}
