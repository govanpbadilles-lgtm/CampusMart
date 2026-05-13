namespace CampusMart.Models.ViewModels.User
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public string? PaymentMethod { get; set; }
        /// <summary>Masked or summary text after simulated GCash / bank entry (e.g. "GCash ••••1234").</summary>
        public string? PaymentAccountMasked { get; set; }
        public string? Notes { get; set; }
        public decimal Total => Items.Sum(i => i.Price * i.Quantity);
    }
}
