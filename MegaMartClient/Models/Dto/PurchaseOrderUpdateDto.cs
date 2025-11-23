using System.ComponentModel.DataAnnotations;

namespace MegaMartClient.Models.Dto
{
    public class PurchaseOrderUpdateDto
    {
        public string Status { get; set; } = string.Empty;
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
        public int SupplierId { get; set; }
    }
}
