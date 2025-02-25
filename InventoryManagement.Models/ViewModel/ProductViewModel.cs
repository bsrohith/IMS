namespace InventoryManagementSystem.Models.ViewModel
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductPhoto { get; set; } = string.Empty;
    }
}
