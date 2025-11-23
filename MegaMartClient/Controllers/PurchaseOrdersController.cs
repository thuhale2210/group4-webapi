using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MegaMartClient.Models.Dto;
using MegaMartClient.Services;
using MegaMartClient.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


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
            var products = await _api.GetProductsAsync();
            var suppliers = await _api.GetSuppliersAsync();

            var productMap = products.ToDictionary(p => p.Id, p => p.Name);
            var supplierMap = suppliers.ToDictionary(s => s.Id, s => s.Name);

            // Attach names to each PO via ViewBag
            ViewBag.ProductNames = productMap;
            ViewBag.SupplierNames = supplierMap;

            var ordered = orders.OrderByDescending(o => o.CreatedAt).ToList();
            return View(ordered);
        }

        // GET: /PurchaseOrders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _api.GetPurchaseOrderAsync(id);
            if (order == null)
                return NotFound();

            // Fetch related names
            var product = await _api.GetProductAsync(order.ProductId);
            var supplier = await _api.GetSupplierAsync(order.SupplierId);

            ViewBag.ProductName = product?.Name ?? "Unknown Product";
            ViewBag.SupplierName = supplier?.Name ?? "Unknown Supplier";

            return View(order);
        }

        // GET: /PurchaseOrders/Create
        // GET: /PurchaseOrders/Create
        public async Task<IActionResult> Create(int? productId)
        {
            var suppliers = await _api.GetSuppliersAsync();
            var products = await _api.GetProductsAsync();

            var vm = new PurchaseOrderFormViewModel
            {
                SupplierOptions = suppliers.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }),
                ProductOptions = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name,
                    Selected = productId.HasValue && productId.Value == p.Id
                }),
                ProductId = productId ?? 0
            };

            if (productId.HasValue)
            {
                var selected = products.FirstOrDefault(p => p.Id == productId.Value);
                if (selected != null)
                {
                    vm.SelectedProductName = selected.Name;
                    vm.SelectedProductCategory = selected.Category;
                    vm.SelectedProductQuantityOnHand = selected.QuantityOnHand;
                    vm.SelectedProductReorderLevel = selected.ReorderLevel;

                    // Simple suggestion: order enough to reach 2x reorder level
                    // e.g., if Reorder = 20 and stock = 5 → suggest 35
                    var targetLevel = selected.ReorderLevel * 2;
                    var suggested = targetLevel - selected.QuantityOnHand;
                    if (suggested < 0) suggested = 0;

                    vm.SuggestedQuantity = suggested;
                    vm.Quantity = suggested; // pre-fill the form
                }
            }

            return View(vm);
        }



        // POST: /PurchaseOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var suppliers = await _api.GetSuppliersAsync();
                var products = await _api.GetProductsAsync();

                vm.SupplierOptions = suppliers.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                });
                vm.ProductOptions = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name,
                    Selected = vm.ProductId == p.Id
                });

                var selected = products.FirstOrDefault(p => p.Id == vm.ProductId);
                if (selected != null)
                {
                    vm.SelectedProductName = selected.Name;
                    vm.SelectedProductCategory = selected.Category;
                    vm.SelectedProductQuantityOnHand = selected.QuantityOnHand;
                    vm.SelectedProductReorderLevel = selected.ReorderLevel;

                    var targetLevel = selected.ReorderLevel * 2;
                    var suggested = targetLevel - selected.QuantityOnHand;
                    if (suggested < 0) suggested = 0;

                    vm.SuggestedQuantity = suggested;
                }

                return View(vm);
            }

            var dto = new PurchaseOrderCreateDto
            {
                SupplierId = vm.SupplierId,
                ProductId = vm.ProductId,
                Quantity = vm.Quantity
            };

            await _api.CreatePurchaseOrderAsync(dto);
            return RedirectToAction(nameof(Index));
        }


        // GET: /PurchaseOrders/Edit/14
        public async Task<IActionResult> Edit(int id)
        {
            var po = await _api.GetPurchaseOrderAsync(id);
            if (po == null)
                return NotFound();

            // for header info
            ViewBag.Id = po.Id;
            ViewBag.CreatedAt = po.CreatedAt;

            // build dropdown lists
            var suppliers = await _api.GetSuppliersAsync();
            var products = await _api.GetProductsAsync();

            ViewBag.SupplierOptions = suppliers.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            });

            ViewBag.ProductOptions = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            var vm = new PurchaseOrderUpdateDto
            {
                Status = po.Status,
                ProductId = po.ProductId,
                Quantity = po.Quantity,
                SupplierId = po.SupplierId
            };

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

                // repopulate dropdowns
                var suppliers = await _api.GetSuppliersAsync();
                var products = await _api.GetProductsAsync();

                ViewBag.SupplierOptions = suppliers.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                });

                ViewBag.ProductOptions = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                });

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

