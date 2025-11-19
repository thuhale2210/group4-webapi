namespace MegaMartClient.Models.Dto
{
    public record PurchaseOrderReadDto(
        int Id,
        DateTime CreatedAt,
        string Status,
        int ProductId,
        int Quantity,
        int SupplierId
    );
}
