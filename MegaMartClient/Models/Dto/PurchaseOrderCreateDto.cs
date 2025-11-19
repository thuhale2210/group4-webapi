namespace MegaMartClient.Models.Dto
{
    public record PurchaseOrderCreateDto(
        int ProductId,
        int Quantity,
        int SupplierId
    );
}
