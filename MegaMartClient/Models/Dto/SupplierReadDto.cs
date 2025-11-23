namespace MegaMartClient.Models.Dto
{
    public class SupplierReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
    }
}