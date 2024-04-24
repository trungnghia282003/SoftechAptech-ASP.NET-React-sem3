using OA.Api.Requests;
using OA.Api.Responses;

namespace OA.Api.Interfaces
{
    public interface IItemRepository
    {
        Task<ItemResponse?> GetByIdItemAsync(int id, int role, int userId);

        Task<CreateItemRequest> CreateAsync(CreateItemRequest model);

        Task<bool> UpdateAsync(CreateItemRequest model);

        Task<ItemResponse[]> GetAllAsync();

        Task<ItemResponse[]> SearchItemAsync(string keyword);

        Task<ItemResponse[]> GetItemsByUserIdAsync(int userId);

        Task<ReportItemResponse[]> ExportReportItemsAsync(ReportItemRequest model, int sellerId);
    }
}
