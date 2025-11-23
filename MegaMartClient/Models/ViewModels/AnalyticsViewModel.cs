namespace MegaMartClient.Models.ViewModels
{
    public class AnalyticsViewModel
    {
        // Products
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }

        // Suppliers
        public int TotalSuppliers { get; set; }

        // Purchase Orders
        public int TotalPurchaseOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ConfirmedOrders { get; set; }
        public int ReceivedOrders { get; set; }
        public int CancelledOrders { get; set; }

        // For charts and lists
        public IList<MegaMartClient.Models.Dto.ProductReadDto> LowStockProductList { get; set; }
            = new List<MegaMartClient.Models.Dto.ProductReadDto>();

        public IList<MegaMartClient.Models.Dto.PurchaseOrderReadDto> RecentOrders { get; set; }
            = new List<MegaMartClient.Models.Dto.PurchaseOrderReadDto>();
    }
}