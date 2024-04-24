using Microsoft.EntityFrameworkCore;
using OA.Api.Interfaces;
using OA.Api.Requests;
using OA.Api.Responses;
using OA.Common;
using OA.Entities;

namespace OA.Api.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly OAContext _context;

        public ItemRepository(OAContext context)
        {
            _context = context;
        }

        public async Task<ItemResponse?> GetByIdItemAsync(int id, int role, int userId)
        {
            var item = await _context.Item.Include(i => i.Rating)
                                          .FirstOrDefaultAsync(i => i.Id == id);
            var buyerBid = await _context.Bid.FirstOrDefaultAsync(i => i.ItemId == id && i.BuyerId == userId);
            return item == null ? null : new ItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ImageName = item.ImageName,
                ImageFile = item.ImageFile,
                DocumentName = item.DocumentName,
                DocumentFile = item.DocumentFile,
                MinIncreasePrice = item.MinIncreasePrice,
                CurrentBidPrice = item.CurrentBidPrice,
                AuctionStartDate = item.AuctionStartDate.Date.ToLocalTime(),
                AuctionEndDate = item.AuctionEndDate.Date.ToLocalTime(),
                SellerId = item.SellerId,
                CategoryId = item.CategoryId,
                BidStatus = item.BidStatus,
                BuyerBid = buyerBid == null ? null : new BidResponse
                {
                    Amount = buyerBid.Amount,
                    IsAchieved = buyerBid.IsAchieved,
                    CreatedDate = buyerBid.CreatedDate.ToLocalTime(),
                },
                ScoreRating = role == (byte)RoleEnums.Buyer ? item.Rating?.SellerScore : item.Rating?.BuyerScore,
                UserId = role == (byte)RoleEnums.Buyer ? item.Rating?.BuyerId : item.Rating?.SellerId
            };
        }

        public async Task<CreateItemRequest> CreateAsync(CreateItemRequest model)
        {
            var item = new Item
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                ImageName = model.ImageName,
                ImageFile = model.ImageFile,
                DocumentName = model.DocumentName,
                DocumentFile = model.DocumentFile,
                MinIncreasePrice = model.MinIncreasePrice,
                CurrentBidPrice = model.CurrentBidPrice,
                AuctionStartDate = model.AuctionStartDate.Date,
                AuctionEndDate = model.AuctionEndDate.Date,
                SellerId = model.SellerId,
                CategoryId = model.CategoryId,
            };

            _context.Item.Add(item);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                model.Id = item.Id;
            }
            return model;
        }

        public async Task<bool> UpdateAsync(CreateItemRequest model)
        {
            var entity = await _context.Item.SingleAsync(ns => ns.Id == model.Id);
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.ImageName = model.ImageName;
            entity.ImageFile = model.ImageFile;
            entity.DocumentName = model.DocumentName;
            entity.DocumentFile = model.DocumentFile;
            entity.MinIncreasePrice = model.MinIncreasePrice;
            entity.CurrentBidPrice = model.CurrentBidPrice;
            entity.AuctionStartDate = model.AuctionStartDate.Date;
            entity.AuctionEndDate = model.AuctionEndDate.Date;
            entity.SellerId = model.SellerId;
            entity.CategoryId = model.CategoryId;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ItemResponse[]> GetAllAsync()
        {
            var listItem = await _context.Item.ToListAsync();
            return listItem.Select(item => new ItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ImageName = item.ImageName,
                ImageFile = item.ImageFile,
                DocumentName = item.DocumentName,
                DocumentFile = item.DocumentFile,
                MinIncreasePrice = item.MinIncreasePrice,
                CurrentBidPrice = item.CurrentBidPrice,
                AuctionStartDate = item.AuctionStartDate.Date,
                AuctionEndDate = item.AuctionEndDate.Date,
                SellerId = item.SellerId,
                CategoryId = item.CategoryId,
                BidStatus = item.BidStatus,
            }).ToArray();
        }

        public async Task<ItemResponse[]> SearchItemAsync(string keyword)
        {
            var listItem = await _context.Item.Where(i => i.Name.Contains(keyword)).ToListAsync();
            return listItem.Select(item => new ItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ImageName = item.ImageName,
                ImageFile = item.ImageFile,
                DocumentName = item.DocumentName,
                DocumentFile = item.DocumentFile,
                MinIncreasePrice = item.MinIncreasePrice,
                CurrentBidPrice = item.CurrentBidPrice,
                AuctionStartDate = item.AuctionStartDate.Date,
                AuctionEndDate = item.AuctionEndDate.Date,
                SellerId = item.SellerId,
                CategoryId = item.CategoryId,
                BidStatus = item.BidStatus,
            }).ToArray();
        }

        public async Task<ItemResponse[]> GetItemsByUserIdAsync(int userId)
        {
            var user = await _context.User.Where(i => i.Id == userId).SingleAsync();
            if (user.Role == (byte)RoleEnums.Buyer)
            {
                return await GetItemsByBuyerIdAsync(userId);
            }
            else if (user.Role == (byte)RoleEnums.Seller)
            {
                return await GetItemsBySellerIdAsync(userId);
            }
            else
            {
                return Array.Empty<ItemResponse>();
            }
        }

        public async Task<ItemResponse[]> GetItemsByBuyerIdAsync(int buyerId)
        {
            var bids = await _context.Bid
                                     .Include(b => b.Item)
                                     .Where(b => b.BuyerId == buyerId)
                                     .ToListAsync();

            return bids.Select(bid => new ItemResponse
            {
                Id = bid.Item.Id,
                Name = bid.Item.Name,
                Description = bid.Item.Description,
                ImageName = bid.Item.ImageName,
                ImageFile = bid.Item.ImageFile,
                DocumentName = bid.Item.DocumentName,
                DocumentFile = bid.Item.DocumentFile,
                MinIncreasePrice = bid.Item.MinIncreasePrice,
                CurrentBidPrice = bid.Item.CurrentBidPrice,
                AuctionStartDate = bid.Item.AuctionStartDate.Date,
                AuctionEndDate = bid.Item.AuctionEndDate.Date,
                SellerId = bid.Item.SellerId,
                CategoryId = bid.Item.CategoryId,
                BidStatus = bid.Item.BidStatus,
                BidResponses = bid.Item.Bids.Select(b => new BidResponse
                {
                    Id = b.Id,
                    BuyerId = b.BuyerId,
                    ItemId = b.ItemId,
                    Amount = b.Amount,
                    IsAchieved = b.IsAchieved
                }).OrderByDescending(b => b.Amount).ToList(),
            }).ToArray();
        }

        public async Task<ItemResponse[]> GetItemsBySellerIdAsync(int sellerId)
        {
            return await _context.Item.Where(i => i.SellerId == sellerId)
                                      .Select(item => new ItemResponse
                                      {
                                          Id = item.Id,
                                          Name = item.Name,
                                          Description = item.Description,
                                          ImageName = item.ImageName,
                                          ImageFile = item.ImageFile,
                                          DocumentName = item.DocumentName,
                                          DocumentFile = item.DocumentFile,
                                          MinIncreasePrice = item.MinIncreasePrice,
                                          CurrentBidPrice = item.CurrentBidPrice,
                                          AuctionStartDate = item.AuctionStartDate.Date,
                                          AuctionEndDate = item.AuctionEndDate.Date,
                                          SellerId = item.SellerId,
                                          CategoryId = item.CategoryId,
                                          BidStatus = item.BidStatus,
                                      }).ToArrayAsync();
        }

        public async Task<ReportItemResponse[]> ExportReportItemsAsync(ReportItemRequest model, int sellerId)
        {
            var result = new List<Item>();

            if (model.BidStatus == (byte)BidStatusEnums.Happening)
            {
                result = await _context.Item.Where(i => i.SellerId == sellerId
                                                     && i.BidStatus == model.BidStatus
                                                     && i.AuctionStartDate.Date >= model.StartDate.Date
                                                     && i.AuctionStartDate.Date <= model.EndDate.Date)
                                            .ToListAsync();
            }
            else
            {
                result = await _context.Item.Include(i => i.Bids)
                                            .ThenInclude(b => b.User)
                                            .Where(i => i.SellerId == sellerId
                                                     && i.BidStatus == model.BidStatus
                                                     && i.AuctionEndDate.Date >= model.StartDate.Date
                                                     && i.AuctionEndDate.Date <= model.EndDate.Date)
                                            .ToListAsync();
            }

            return result.Select(item => new ReportItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                MinIncreasePrice = item.MinIncreasePrice,
                CurrentBidPrice = item.CurrentBidPrice,
                AuctionStartDate = item.AuctionStartDate,
                AuctionEndDate = item.AuctionEndDate,
                BuyerName = item.Bids?.FirstOrDefault(b => b.IsAchieved)?.User?.FullName
            }).ToArray();
        }

    }
}
