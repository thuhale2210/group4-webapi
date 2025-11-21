using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MegaMartClient.Models.Dto;
using MegaMartClient.Services;

namespace MegaMartClient.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private readonly ISmartStockApiClient _api;

        public PurchaseOrdersController(ISmartStockApiClient api)
        {
            _api = api;
        }

        // GET: /PurchaseOrders
        public async Task<IActionResult> Index()
        {
            var orders = await _api.GetPurchaseOrdersAsync();
            var ordered = orders.OrderByDescending(o => o.CreatedAt).ToList();
            return View(ordered);
        }

        // GET: /PurchaseOrders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _api.GetPurchaseOrderAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: /PurchaseOrders/Create
        public IActionResult Create(int? productId, int? supplierId)
        {
            var vm = new PurchaseOrderCreateDto
            {
                ProductId = productId ?? 0,
                SupplierId = supplierId ?? 0,
                Quantity = 1
            };

            return View(vm);
        }

        // POST: /PurchaseOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _api.CreatePurchaseOrderAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /PurchaseOrders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _api.GetPurchaseOrderAsync(id);
            if (order == null) return NotFound();

            var vm = new PurchaseOrderUpdateDto
            {
                Status = order.Status,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                SupplierId = order.SupplierId
            };

            ViewBag.Id = id;
            ViewBag.CreatedAt = order.CreatedAt;

            return View(vm);
        }

        // POST: /PurchaseOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseOrderUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(dto);
            }

            await _api.UpdatePurchaseOrderAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /PurchaseOrders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _api.GetPurchaseOrderAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: /PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeletePurchaseOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}



//using Microsoft.AspNetCore.Mvc;
//using MegaMartClient.Models.Dto;

//namespace MegaMartClient.Controllers
//{
//    public class PurchaseOrdersController : Controller
//    {
//        private static readonly List<PurchaseOrderReadDto> mockOrders = new()
//        {
//            new PurchaseOrderReadDto
//            {
//                Id = 1,
//                CreatedAt = DateTime.Today.AddDays(-3),
//                Status = "Received",
//                ProductId = 1,
//                Quantity = 10,
//                SupplierId = 1
//            },
//            new PurchaseOrderReadDto
//            {
//                Id = 2,
//                CreatedAt = DateTime.Today.AddDays(-1),
//                Status = "Pending",
//                ProductId = 2,
//                Quantity = 5,
//                SupplierId = 2
//            },
//            new PurchaseOrderReadDto
//            {
//                Id = 3,
//                CreatedAt = DateTime.Today.AddDays(-2),
//                Status = "Confirmed",
//                ProductId = 3,
//                Quantity = 8,
//                SupplierId = 3
//            }
//        };

//        public IActionResult Index()
//        {
//            return View(mockOrders.OrderByDescending(o => o.CreatedAt));
//        }

//        public IActionResult Details(int id)
//        {
//            var order = mockOrders.FirstOrDefault(o => o.Id == id);
//            if (order == null) return NotFound();
//            return View(order);
//        }

//        public IActionResult Create(
//    int? productId,
//    int? supplierId,
//    string? productName,
//    int? currentQty,
//    int? reorderLevel)
//        {
//            // Suggested quantity: use reorder level if available, otherwise 1
//            int suggestedQty = reorderLevel ?? 1;

//            var vm = new PurchaseOrderCreateDto
//            {
//                ProductId = productId ?? 0,
//                Quantity = suggestedQty,
//                SupplierId = supplierId ?? 0
//            };

//            // Pass extra info to the view for display
//            ViewBag.ProductName = productName ?? $"Product #{productId ?? 0}";
//            ViewBag.CurrentQty = currentQty;
//            ViewBag.ReorderLevel = reorderLevel;

//            return View(vm);
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(PurchaseOrderCreateDto dto)
//        {
//            if (!ModelState.IsValid)
//                return View(dto);

//            var nextId = mockOrders.Any() ? mockOrders.Max(o => o.Id) + 1 : 1;

//            var newOrder = new PurchaseOrderReadDto
//            {
//                Id = nextId,
//                CreatedAt = DateTime.Now,
//                Status = "Pending",
//                ProductId = dto.ProductId,
//                Quantity = dto.Quantity,
//                SupplierId = dto.SupplierId
//            };

//            mockOrders.Add(newOrder);

//            return RedirectToAction(nameof(Index));
//        }

//        public IActionResult Edit(int id)
//        {
//            var order = mockOrders.FirstOrDefault(o => o.Id == id);
//            if (order == null) return NotFound();

//            ViewBag.Id = id;
//            ViewBag.CreatedAt = order.CreatedAt;

//            return View(new PurchaseOrderUpdateDto
//            {
//                Status = order.Status,
//                ProductId = order.ProductId,
//                Quantity = order.Quantity,
//                SupplierId = order.SupplierId
//            });
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Edit(int id, PurchaseOrderUpdateDto dto)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.Id = id;
//                return View(dto);
//            }

//            var order = mockOrders.FirstOrDefault(o => o.Id == id);
//            if (order == null) return NotFound();

//            order.Status = dto.Status;
//            order.ProductId = dto.ProductId;
//            order.Quantity = dto.Quantity;
//            order.SupplierId = dto.SupplierId;

//            return RedirectToAction(nameof(Index));
//        }

//        public IActionResult Delete(int id)
//        {
//            var order = mockOrders.FirstOrDefault(o => o.Id == id);
//            if (order == null) return NotFound();
//            return View(order);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            var order = mockOrders.FirstOrDefault(o => o.Id == id);
//            if (order != null) mockOrders.Remove(order);

//            return RedirectToAction(nameof(Index));
//        }
//    }
//}

