using CampusMart.Models.Entities;

namespace CampusMart.Models.ViewModels.User
{
    public class RentalPageViewModel
    {
        public List<RentalCategory> Categories { get; set; } = new();
        public List<RentalItem> AvailableItems { get; set; } = new();
        public List<Rental> UserActiveRentals { get; set; } = new();
        public int? SelectedCategoryId { get; set; }
    }
}
