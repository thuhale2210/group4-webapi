namespace MegaMartClient.Models.Dto
{
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }

        public string? ImageUrl { get; set; }
    }
}
