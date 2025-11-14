namespace FreshSourceAPI.DTOs
{
    public record SupplierReadDto(
        int Id,
        string Name,
        string ContactEmail,
        string Phone
    );

    public record SupplierCreateDto(
        string Name,
        string ContactEmail,
        string Phone
    );

    public record SupplierUpdateDto(
        string Name,
        string ContactEmail,
        string Phone
    );
}
