namespace FreshSourceAPI.Entities
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Pending | Confirmed | Received | Cancelled
        public string Status { get; set; } = "Pending";

        // Product being ordered
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }

        // Supplier fulfilling this order
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
    }
}
