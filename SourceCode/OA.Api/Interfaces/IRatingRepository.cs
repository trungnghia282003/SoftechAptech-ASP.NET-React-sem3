using OA.Api.Requests;
using OA.Api.Responses;

namespace OA.Api.Interfaces
{
    public interface IRatingRepository
    {
        Task<bool> UpdateAsync(RatingRequest model, int userId);

        Task<ReportRatingResponse[]> GetAllRatingAsync();
    }
}
