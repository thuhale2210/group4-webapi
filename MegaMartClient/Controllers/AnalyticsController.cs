using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MegaMartClient.Models.ViewModels;
using MegaMartClient.Services;

namespace MegaMartClient.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly ISmartStockApiClient _api;

        public AnalyticsController(ISmartStockApiClient api)
        {
            _api = api;
        }

        // GET: /Analytics
        public async Task<IActionResult> Index()
        {
            // Fetch from API
            var products = (await _api.GetProductsAsync()).ToList();
            var suppliers = (await _api.GetSuppliersAsync()).ToList();
            var purchaseOrders = (await _api.GetPurchaseOrdersAsync()).ToList();

            var lowStockProducts = products
                .Where(p => p.QuantityOnHand <= p.ReorderLevel)
                .ToList();

            var orderedPOs = purchaseOrders
                .OrderByDescending(po => po.CreatedAt)
                .ToList();

            var vm = new AnalyticsViewModel
            {
                // Products
                TotalProducts = products.Count,
                LowStockProducts = lowStockProducts.Count,
                LowStockProductList = lowStockProducts,

                // Suppliers
                TotalSuppliers = suppliers.Count,

                // Purchase Orders Status
                TotalPurchaseOrders = purchaseOrders.Count,
                PendingOrders = purchaseOrders.Count(po => po.Status == "Pending"),
                ConfirmedOrders = purchaseOrders.Count(po => po.Status == "Confirmed"),
                ReceivedOrders = purchaseOrders.Count(po => po.Status == "Received"),
                CancelledOrders = purchaseOrders.Count(po => po.Status == "Cancelled"),

                // Take top 5 recent orders
                RecentOrders = orderedPOs.Take(5).ToList()
            };

            return View(vm);
        }
    }
}
