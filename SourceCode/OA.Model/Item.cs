using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OA.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string? Description { get; set; }

        [Required]
        [MaxLength(200)]
        public string? ImageName { get; set; }

        [Required]
        public byte[]? ImageFile { get; set; }

        [MaxLength(200)]
        public string? DocumentName { get; set; }

        public byte[]? DocumentFile { get; set; }

        [Column(TypeName = "money")]
        public decimal MinIncreasePrice { get; set; }

        [Column(TypeName = "money")]
        public decimal CurrentBidPrice { get; set; }

        public DateTime AuctionStartDate { get; set; }

        public DateTime AuctionEndDate { get; set; }

        [ForeignKey(nameof(User))]
        public int SellerId { get; set; }
        public User? User { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public byte BidStatus { get; set; }

        public ICollection<Bid>? Bids { get; set; }

        public Rating? Rating { get; set; }
    }
}
