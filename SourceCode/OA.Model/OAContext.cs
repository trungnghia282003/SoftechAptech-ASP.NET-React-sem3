using Microsoft.EntityFrameworkCore;

namespace OA.Entities
{
    public class OAContext : DbContext
    {
        public OAContext(DbContextOptions<OAContext> options) : base(options)
        {
        }

        public DbSet<Bid> Bid { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Item> Item { get; set; } = default!;
        public DbSet<Notification> Notification { get; set; } = default!;
        public DbSet<Rating> Rating { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Buyer)
                .WithMany(u => u.BuyerRatings)
                .HasForeignKey(r => r.BuyerId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Seller)
                .WithMany(u => u.SellerRatings)
                .HasForeignKey(r => r.SellerId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Item)
                .WithMany()
                .HasForeignKey(n => n.ItemId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.SellerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.BuyerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Item)
                .WithMany(i => i.Bids)
                .HasForeignKey(b => b.ItemId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Category)
                .HasForeignKey(i => i.CategoryId);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Rating)
                .WithOne(r => r.Item)
                .HasForeignKey<Rating>(r => r.ItemId);
        }
    }
}
