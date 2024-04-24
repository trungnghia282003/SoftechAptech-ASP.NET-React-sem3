using OA.Api.Requests;
using OA.Api.Responses;

namespace OA.Api.Interfaces
{
    public interface ICategoryRepository
    {
        Task<CategoryResponse[]> GetAllAsync(int role);

        Task<CategoryResponse[]> GetAllCategoryAsync(bool isActive);

        Task<CategoryResponse?> GetAllItemByCategoryIdAsync(int id, int pageNumber);

        Task<CreateCategoryRequest> UpdateAsync(CreateCategoryRequest model);

        Task<CreateCategoryRequest> CreateAsync(CreateCategoryRequest model);
    }
}
