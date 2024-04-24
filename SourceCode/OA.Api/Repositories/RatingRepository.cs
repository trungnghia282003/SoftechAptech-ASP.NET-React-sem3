using Microsoft.EntityFrameworkCore;
using OA.Api.Interfaces;
using OA.Api.Requests;
using OA.Api.Responses;
using OA.Common;
using OA.Entities;

namespace OA.Api.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly OAContext _context;

        public RatingRepository(OAContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateAsync(RatingRequest model, int role)
        {
            var entity = await _context.Rating.SingleAsync(ns => ns.ItemId == model.ItemId);
            if (role == (byte)RoleEnums.Buyer)
            {
                entity.SellerScore = model.Score;
            }
            else
            {
                entity.BuyerScore = model.Score;
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ReportRatingResponse[]> GetAllRatingAsync()
        {
            var listRatings = await _context.Rating
                                            .Include(r => r.Seller)
                                            .Include(r => r.Buyer)
                                            .ToListAsync();

            var sellerRatings = listRatings
                .Where(r => r.SellerId != null && r.SellerScore != null)
                .GroupBy(r => new { r.SellerId, r.Seller.FullName })
                .Select(g => new ReportRatingResponse
                {
                    Id = g.Key.SellerId,
                    Name = g.Key.FullName,
                    Role = RoleEnums.Seller.ToString(),
                    AverageOfScore = Math.Round(g.Average(r => r.SellerScore).Value, 2, MidpointRounding.AwayFromZero)
                });

            var buyerRatings = listRatings
                .Where(r => r.BuyerId != null && r.BuyerScore != null)
                .GroupBy(r => new { r.BuyerId, r.Buyer.FullName })
                .Select(g => new ReportRatingResponse
                {
                    Id = g.Key.BuyerId,
                    Name = g.Key.FullName,
                    Role = RoleEnums.Buyer.ToString(),
                    AverageOfScore = Math.Round(g.Average(r => r.BuyerScore).Value, 2, MidpointRounding.AwayFromZero)
                });

            return sellerRatings.Concat(buyerRatings)
                                .OrderByDescending(r => r.AverageOfScore)
                                .ToArray();
        }
    }
}
