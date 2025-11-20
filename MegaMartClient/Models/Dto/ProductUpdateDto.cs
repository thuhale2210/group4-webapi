namespace MegaMartClient.Models.Dto
{
    public class ProductUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }
        public string? ImageUrl { get; set; }
        public int SupplierId { get; set; }
    }
}
