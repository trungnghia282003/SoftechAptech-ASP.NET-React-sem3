using Microsoft.EntityFrameworkCore;
using OA.Api.Interfaces;
using OA.Api.Requests;
using OA.Api.Responses;
using OA.Common;
using OA.Entities;

namespace OA.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly OAContext _context;

        public CategoryRepository(OAContext context)
        {
            _context = context;
        }

        public async Task<CategoryResponse[]> GetAllAsync(int role)
        {
            if (role != (byte)RoleEnums.Admin)
            {
                return Array.Empty<CategoryResponse>();
            }

            var categories = await _context.Category.ToListAsync();
            return categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            }).ToArray();
        }

        public async Task<CategoryResponse[]> GetAllCategoryAsync(bool isActive)
        {
            var categories = await _context.Category.Where(c => c.IsActive == isActive).ToListAsync();
            return categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            }).ToArray();
        }

        public async Task<CategoryResponse?> GetAllItemByCategoryIdAsync(int id, int pageNumber)
        {
            var category = await _context.Category
                                        .Include(i => i.Items)
                                        .FirstOrDefaultAsync(i => i.Id == id);
            if (category == null)
            {
                return null;
            }

            List<ItemResponse> listItem = new List<ItemResponse>();
            if (category.Items != null)
            {
                listItem = category.Items
                                   .OrderBy(i => i.Id)
                                   .Skip((pageNumber - 1) * 4)
                                   .Take(4)
                                   .Select(b => new ItemResponse
                                   {
                                       Id = b.Id,
                                       Name = b.Name,
                                       Description = b.Description,
                                       ImageName = b.ImageName,
                                       ImageFile = b.ImageFile,
                                       DocumentName = b.DocumentName,
                                       DocumentFile = b.DocumentFile,
                                       MinIncreasePrice = b.MinIncreasePrice,
                                       CurrentBidPrice = b.CurrentBidPrice,
                                       AuctionStartDate = b.AuctionStartDate,
                                       AuctionEndDate = b.AuctionEndDate,
                                       SellerId = b.SellerId,
                                       CategoryId = b.CategoryId,
                                       BidStatus = b.BidStatus,
                                   }).ToList();
            }

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                ItemResponses = listItem
            };
        }

        public async Task<CreateCategoryRequest?> UpdateAsync(CreateCategoryRequest model)
        {
            if (model.Role != (byte)RoleEnums.Admin)
            {
                return null;
            }

            var entity = await _context.Category.SingleAsync(ns => ns.Id == model.Id);
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.IsActive = model.IsActive;
            var result = await _context.SaveChangesAsync();
            return model;
        }

        public async Task<CreateCategoryRequest> CreateAsync(CreateCategoryRequest model)
        {
            if (model.Role != (byte)RoleEnums.Admin)
            {
                return null;
            }

            var category = new Category
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
            };

            _context.Category.Add(category);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                model.Id = category.Id;
            }
            return model;
        }
    }
}
