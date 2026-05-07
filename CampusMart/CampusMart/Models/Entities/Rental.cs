using System.ComponentModel.DataAnnotations;

namespace CampusMart.Models.Entities
{
    public class Rental
    {
        public int Id { get; set; }

        // FK → ApplicationUser
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // FK → RentalItem
        public int RentalItemId { get; set; }
        public RentalItem RentalItem { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>"Active", "Returned", "Overdue", "Cancelled"</summary>
        public string Status { get; set; } = "Active";

        public decimal TotalPrice { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
