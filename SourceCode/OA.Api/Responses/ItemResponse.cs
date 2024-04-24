namespace OA.Api.Responses
{
    public class ItemResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ImageName { get; set; }

        public byte[]? ImageFile { get; set; }

        public string? DocumentName { get; set; }

        public byte[]? DocumentFile { get; set; }

        public decimal MinIncreasePrice { get; set; }

        public decimal CurrentBidPrice { get; set; }

        public DateTime AuctionStartDate { get; set; }

        public DateTime AuctionEndDate { get; set; }

        public int? SellerId { get; set; }

        public int CategoryId { get; set; }

        public byte BidStatus { get; set; }

        public List<BidResponse>? BidResponses { get; set; }

        public BidResponse? BuyerBid { get; set; }

        public int? ScoreRating { get; set; }

        public int? UserId { get; set; }
    }
}
