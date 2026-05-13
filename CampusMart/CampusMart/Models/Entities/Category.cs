using System.ComponentModel.DataAnnotations;

namespace CampusMart.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = "";

        public string? Icon { get; set; }

        public string? Description { get; set; }

        // FK → Stall (Each stall can have its own categories)
        public int? StallId { get; set; }
        public Stall? Stall { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
