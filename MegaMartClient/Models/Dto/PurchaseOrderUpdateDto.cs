namespace MegaMartClient.Models.Dto
{
    public class PurchaseOrderUpdateDto
    {
        public string Status { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int SupplierId { get; set; }
    }
}
