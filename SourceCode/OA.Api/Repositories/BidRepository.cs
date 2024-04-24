using Microsoft.EntityFrameworkCore;
using OA.Api.Interfaces;
using OA.Api.Requests;
using OA.Api.Responses;
using OA.Entities;

namespace OA.Api.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly OAContext _context;

        public BidRepository(OAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(CreateBidRequest model)
        {
            try
            {
                var bidItem = new Bid
                {
                    Id = model.Id,
                    BuyerId = model.BuyerId,
                    ItemId = model.ItemId,
                    Amount = model.Amount,
                    CreatedDate = DateTime.Now.ToUniversalTime(),
                    IsAchieved = model.IsAchieved,
                };

                var entity = await _context.Item.SingleAsync(ns => ns.Id == model.ItemId);
                entity.CurrentBidPrice = model.Amount;

                _context.Bid.Add(bidItem);
                var result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return false;
            }
        }
        public async Task<BidResponse[]> GetAllBid(int itemId)
        {
            var bids = await _context.Bid.Where(c => c.ItemId == itemId).ToListAsync();
            return bids.Select(bid => new BidResponse
            {
                Id = bid.Id,
                ItemId = bid.ItemId,
                BuyerId = bid.BuyerId,
                Amount = bid.Amount,
                CreatedDate = bid.CreatedDate.ToLocalTime(),
            }).OrderByDescending(b => b.Amount).ToArray();
        }
    }
}
