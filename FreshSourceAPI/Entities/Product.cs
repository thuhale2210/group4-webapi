namespace FreshSourceAPI.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal UnitPrice { get; set; }

        // Inventory info
        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }

        // S3 image URL
        public string? ImageUrl { get; set; }

        // Relationship to Supplier
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
    }
}
