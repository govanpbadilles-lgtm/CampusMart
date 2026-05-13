using System.ComponentModel.DataAnnotations;

namespace CampusMart.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = "";

        [Required]
        public decimal Price { get; set; } = decimal.Zero;

        [Required]
        public int Stock { get; set; } = 0;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        // FK → Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // FK → Stall (products belong to stalls, managed by admin)
        public int StallId { get; set; }
        public Stall? Stall { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
