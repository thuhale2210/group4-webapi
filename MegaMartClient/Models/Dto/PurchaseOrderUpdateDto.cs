namespace MegaMartClient.Models.Dto
{
    public record PurchaseOrderUpdateDto(
        string Status,
        int ProductId,
        int Quantity,
        int SupplierId
    );
}
