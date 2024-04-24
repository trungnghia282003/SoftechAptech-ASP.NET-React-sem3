using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OA.Entities
{
    public class Bid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int BuyerId { get; set; }
        public User? User { get; set; }

        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        public Item? Item { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsAchieved { get; set; }
    }
}
