using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models.ViewModel
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock Quantity must be at least 1")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        public int SupplierId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string ProductDescription { get; set; } = string.Empty;

        public string ProductPhoto { get; set; } = string.Empty;
    }

}
