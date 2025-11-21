namespace MegaMartClient.Models.Dto
{
    public class PurchaseOrderCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int SupplierId { get; set; }
    }
}
