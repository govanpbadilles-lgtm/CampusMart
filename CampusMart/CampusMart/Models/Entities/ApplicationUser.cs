using Microsoft.AspNetCore.Identity;


namespace CampusMart.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
    }
}
