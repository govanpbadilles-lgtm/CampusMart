using System.ComponentModel.DataAnnotations;

namespace CampusMart.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = "";
        public ApplicationUser? User { get; set; }

        public string Status { get; set; } = "Pending"; // "Pending", "Confirmed", "Completed", "Cancelled"

        [Required]
        public decimal TotalAmount { get; set; } = 0;

        public string? ShippingAddress { get; set; }
        public string? PaymentMethod { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
