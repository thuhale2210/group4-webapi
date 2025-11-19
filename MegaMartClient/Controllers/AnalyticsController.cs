using Microsoft.AspNetCore.Mvc;

namespace MegaMartClient.Controllers
{
    public class AnalyticsController : Controller
    {
        // GET: /Analytics
        public IActionResult Index()
        {
            // TEMP: mock stats for the dashboard
            ViewBag.TotalProducts = 12;
            ViewBag.LowStockCount = 3;
            ViewBag.TotalSuppliers = 5;
            ViewBag.OpenPurchaseOrders = 4;

            return View();
        }
    }
}
