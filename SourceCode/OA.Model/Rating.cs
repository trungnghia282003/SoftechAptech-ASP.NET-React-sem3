using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OA.Entities
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        public Item? Item { get; set; }

        [ForeignKey(nameof(User))]
        public int? SellerId { get; set; }
        public User? Seller { get; set; }

        public int? SellerScore { get; set; }

        [ForeignKey(nameof(User))]
        public int? BuyerId { get; set; }
        public User? Buyer { get; set; }

        public int? BuyerScore { get; set; }
    }
}
