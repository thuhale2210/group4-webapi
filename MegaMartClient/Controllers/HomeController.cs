using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MegaMartClient.Models.ViewModels;
using MegaMartClient.Services;

namespace MegaMartClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISmartStockApiClient _api;

        public HomeController(ISmartStockApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch from API
            var products = (await _api.GetProductsAsync()).ToList();
            var suppliers = (await _api.GetSuppliersAsync()).ToList();
            var orders = (await _api.GetPurchaseOrdersAsync()).ToList();

            // Overall Counts
            var totalProducts = products.Count;
            var lowStock = products.Count(p => p.QuantityOnHand <= p.ReorderLevel);
            var supplierCount = suppliers.Count;
            var openPoCount = orders.Count(o => o.Status == "Pending" || o.Status == "Confirmed");

            // Low stock products
            var lowStockProducts = products
                .Where(p => p.QuantityOnHand <= p.ReorderLevel)
                .ToList();

            // Stock quantity by category
            var totalQty = products.Sum(p => p.QuantityOnHand);
            var categoryBreakdown = products
                .GroupBy(p => p.Category ?? "Other")
                .Select(g =>
                {
                    var qty = g.Sum(p => p.QuantityOnHand);
                    var pct = totalQty > 0
                        ? (int)Math.Round(100.0 * qty / totalQty)
                        : 0;
                    return new CategoryStockItem
                    {
                        Category = g.Key,
                        Percentage = pct
                    };
                })
                .OrderByDescending(c => c.Percentage)
                .ToList();

            // last Weeks Purchase orders
            var today = DateOnly.FromDateTime(DateTime.Today);
            var sevenDaysAgo = today.AddDays(-6);

            var last7Days = orders
                .Where(o => DateOnly.FromDateTime(o.CreatedAt) >= sevenDaysAgo)
                .GroupBy(o => DateOnly.FromDateTime(o.CreatedAt))
                .Select(g =>
                {
                    var countsByStatus = g
                        .GroupBy(o => o.Status ?? "Unknown")
                        .OrderByDescending(x => x.Count())
                        .ToList();

                    var dominantStatus = countsByStatus.First().Key;

                    return new DailyPoStat
                    {
                        Date = g.Key,
                        PoCount = g.Count(),
                        StatusLabel = dominantStatus
                    };
                })
                .OrderByDescending(d => d.Date)
                .Take(7)
                .ToList();

            var vm = new DashboardViewModel
            {
                TotalProducts = totalProducts,
                LowStockItems = lowStock,
                SupplierCount = supplierCount,
                OpenPurchaseOrders = openPoCount,
                CategoryBreakdown = categoryBreakdown,
                Last7Days = last7Days,
                LowStockProducts = lowStockProducts
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
