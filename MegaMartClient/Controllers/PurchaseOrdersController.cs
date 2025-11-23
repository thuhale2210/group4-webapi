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
            // Fetch from API
            var orders = await _api.GetPurchaseOrdersAsync();
            var products = await _api.GetProductsAsync();
            var suppliers = await _api.GetSuppliersAsync();

            // lookup dictionary
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

                    // Suggestion amount to order
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

        // GET: /PurchaseOrders/Edit
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


        // POST: /PurchaseOrders/Edit
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

        // PATCH: /PurchaseOrders/QuickUpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickUpdateStatus(int id, string status)
        {
            await _api.PatchPurchaseOrderStatusAsync(id, status);
            return RedirectToAction("Index");
        }

        // GET: /PurchaseOrders/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _api.GetPurchaseOrderAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: /PurchaseOrders/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeletePurchaseOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}