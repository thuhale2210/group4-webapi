namespace FreshSourceAPI.DTOs
{
    public record ProductReadDto(
        int Id,
        string Name,
        string Category,
        decimal UnitPrice,
        int QuantityOnHand,
        int ReorderLevel,
        string? ImageUrl,
        int SupplierId
    );
    public record ProductCreateDto(
        string Name,
        string Category,
        decimal UnitPrice,
        int QuantityOnHand,
        int ReorderLevel,
        string? ImageUrl,
        int SupplierId
    );
    public record ProductUpdateDto(
        string Name,
        string Category,
        decimal UnitPrice,
        int QuantityOnHand,
        int ReorderLevel,
        string? ImageUrl,
        int SupplierId
    );
}
