namespace OA.Api.Responses
{
    public class NotificationResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ItemId { get; set; }

        public string? Message { get; set; }

        public DateTime NotificationDate { get; set; }

        public bool MarkRead { get; set; }
    }
}
