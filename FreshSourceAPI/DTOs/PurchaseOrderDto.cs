namespace FreshSourceAPI.DTOs
{
    public record PurchaseOrderReadDto(
        int Id,
        DateTime CreatedAt,
        string Status,
        int ProductId,
        int Quantity,
        int SupplierId
    );

    public record PurchaseOrderCreateDto(
        int ProductId,
        int Quantity,
        int SupplierId
    );

    public record PurchaseOrderUpdateDto(
        string Status,
        int ProductId,
        int Quantity,
        int SupplierId
    );
}
