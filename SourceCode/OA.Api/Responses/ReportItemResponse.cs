namespace OA.Api.Responses
{
    public class ReportItemResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal MinIncreasePrice { get; set; }

        public decimal CurrentBidPrice { get; set; }

        public DateTime AuctionStartDate { get; set; }

        public DateTime AuctionEndDate { get; set; }

        public string? BuyerName { get; set; }
    }
}
