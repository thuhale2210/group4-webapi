namespace MegaMartClient.Models.Dto
{
    public record SupplierReadDto(
        int Id,
        string Name,
        string ContactEmail,
        string Phone
    );
}
