using System.ComponentModel.DataAnnotations;

namespace OA.Api.Requests
{
    public class CreateItemRequest
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [MaxLength(200)]
        public string? ImageName { get; set; }

        [Required]
        public byte[]? ImageFile { get; set; }

        [MaxLength(200)]
        public string? DocumentName { get; set; }

        public byte[]? DocumentFile { get; set; }

        public decimal MinIncreasePrice { get; set; }

        public decimal CurrentBidPrice { get; set; }

        public DateTime AuctionStartDate { get; set; }

        public DateTime AuctionEndDate { get; set; }

        public int SellerId { get; set; }

        public int CategoryId { get; set; }
    }
}
