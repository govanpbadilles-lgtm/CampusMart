using System.ComponentModel.DataAnnotations;

namespace CampusMart.Models.Entities
{
    public class RentalCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        /// <summary>Emoji icon for UI display (e.g. "🔬", "🏢")</summary>
        public string Icon { get; set; } = "📦";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RentalItem> RentalItems { get; set; } = new List<RentalItem>();
    }
}
