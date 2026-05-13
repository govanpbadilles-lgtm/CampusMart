using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CampusMart.Models.Entities;

namespace CampusMart.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // Floor & Stall Management
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Stall> Stalls { get; set; }

        // Saved Items
        public DbSet<SavedItem> SavedItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cart → User (one-to-one)
            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId);

            // Transaction → User
            builder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId);

            // Transaction → Product
            builder.Entity<Transaction>()
                .HasOne(t => t.Product)
                .WithMany()
                .HasForeignKey(t => t.ProductId);

            // Order → User
            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // CartItem → Product
            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);

            // OrderItem → Product
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            // Product → Category
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Product → Stall
            builder.Entity<Product>()
                .HasOne(p => p.Stall)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StallId)
                .OnDelete(DeleteBehavior.Restrict);

            // Floor → Stalls (one-to-many)
            builder.Entity<Stall>()
                .HasOne(s => s.Floor)
                .WithMany(f => f.Stalls)
                .HasForeignKey(s => s.FloorId)
                .OnDelete(DeleteBehavior.Cascade);

            // SavedItem → User
            builder.Entity<SavedItem>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // SavedItem → Product
            builder.Entity<SavedItem>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Category → Stall
            builder.Entity<Category>()
                .HasOne(c => c.Stall)
                .WithMany(s => s.Categories)
                .HasForeignKey(c => c.StallId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Textbooks & Academics", Description = "Academic books and study materials", Icon = "📚", StallId = null },
                new Category { Id = 2, Name = "Electronics & Gadgets", Description = "Tech devices and accessories", Icon = "💻", StallId = null },
                new Category { Id = 3, Name = "Clothing & Apparel", Description = "Campus fashion and merch", Icon = "👕", StallId = null },
                new Category { Id = 4, Name = "Stationery & Supplies", Description = "Writing instruments and planners", Icon = "✏️", StallId = null },
                new Category { Id = 5, Name = "Food & Beverages", Description = "Snacks, drinks and meal items", Icon = "🍱", StallId = null },
                new Category { Id = 6, Name = "Other", Description = "Miscellaneous items", Icon = "📦", StallId = null }
            );

            // Set precision for decimal properties to avoid warnings
            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
        }
    }
}
