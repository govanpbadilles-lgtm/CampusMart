using System.ComponentModel.DataAnnotations;

namespace CampusMart.Models.Entities
{
    public class RentalItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public decimal PricePerUnit { get; set; }

        /// <summary>Pricing unit: "hr", "day", "week", "mo"</summary>
        [Required]
        [StringLength(10)]
        public string PriceUnit { get; set; } = "day";

        public string? Location { get; set; }

        public int TotalStock { get; set; } = 1;

        public int AvailableStock { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        // FK → RentalCategory
        public int RentalCategoryId { get; set; }
        public RentalCategory RentalCategory { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
