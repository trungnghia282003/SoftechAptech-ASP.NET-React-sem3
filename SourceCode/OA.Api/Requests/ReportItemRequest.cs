namespace OA.Api.Requests
{
    public class ReportItemRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte BidStatus { get; set; }
    }
}
