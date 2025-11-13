namespace FreshSourceAPI.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}
