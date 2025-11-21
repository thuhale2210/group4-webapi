namespace MegaMartClient.Models.Dto
{
    public class PurchaseOrderReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int SupplierId { get; set; }
    }
}