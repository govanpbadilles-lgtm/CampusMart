using System.ComponentModel.DataAnnotations;

namespace CampusMart.Models.Entities
{
    public class SavedItem
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? ProductId { get; set; }
        public Product Product { get; set; }

        public int? StallItemId { get; set; }
        public StallItem StallItem { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
