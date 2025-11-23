using MegaMartClient.Models.Dto;
using MegaMartClient.Models.ViewModels;
using MegaMartClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MegaMartClient.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ISmartStockApiClient _api;

        public ProductsController(ISmartStockApiClient api)
        {
            _api = api;
        }

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            // Fetch from API
            var products = await _api.GetProductsAsync();
            var suppliers = await _api.GetSuppliersAsync();

            // lookup dictionary
            var supplierMap = suppliers.ToDictionary(s => s.Id, s => s.Name);

            // attach supplier names into each ProductReadDto
            foreach (var product in products)
            {
                if (supplierMap.TryGetValue(product.SupplierId, out var supplierName))
                    product.SupplierName = supplierName;
                else
                    product.SupplierName = "Unknown Supplier";
            }

            return View(products);
        }

        // GET: /Products/Details
        public async Task<IActionResult> Details(int id)
        {
            var product = await _api.GetProductAsync(id);
            if (product == null)
                return NotFound();

            // Fetch supplier name
            var supplier = await _api.GetSupplierAsync(product.SupplierId);
            ViewBag.SupplierName = supplier?.Name ?? "Unknown Supplier";

            return View(product);
        }

        // GET: /Products/Create
        public async Task<IActionResult> Create()
        {
            var suppliers = await _api.GetSuppliersAsync();

            var vm = new ProductFormViewModel
            {
                SupplierOptions = suppliers.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
            };

            return View(vm);
        }

        // POST: /Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.SupplierOptions = (await _api.GetSuppliersAsync())
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    });

                return View(vm);
            }

            var dto = new ProductCreateDto
            {
                Name = vm.Name,
                Category = vm.Category,
                UnitPrice = vm.UnitPrice,
                QuantityOnHand = vm.QuantityOnHand,
                ReorderLevel = vm.ReorderLevel,
                SupplierId = vm.SupplierId,
                ImageUrl = vm.ImageUrl
            };

            await _api.CreateProductAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _api.GetProductAsync(id);
            if (product == null)
                return NotFound();

            // fetch suppliers for dropdown
            var suppliers = await _api.GetSuppliersAsync();

            ViewBag.SupplierOptions = suppliers.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            });

            var vm = new ProductUpdateDto
            {
                Name = product.Name,
                Category = product.Category,
                UnitPrice = product.UnitPrice,
                QuantityOnHand = product.QuantityOnHand,
                ReorderLevel = product.ReorderLevel,
                SupplierId = product.SupplierId,
                ImageUrl = product.ImageUrl
            };

            ViewBag.Id = id;
            return View(vm);
        }

        // POST: /Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                // repopulate dropdown if form fails validation
                var suppliers = await _api.GetSuppliersAsync();
                ViewBag.SupplierOptions = suppliers.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                });

                ViewBag.Id = id;
                return View(dto);
            }

            await _api.UpdateProductAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        // PATCH: /Products/QuickUpdateQoH
        [HttpPost]
        public async Task<IActionResult> QuickUpdateQoH(int id, int newQoH)
        {
            if (newQoH < 0)
            {
                ModelState.AddModelError("", "Quantity cannot be negative.");
                return RedirectToAction(nameof(Index));
            }

            await _api.PatchProductQuantityAsync(id, newQoH);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _api.GetProductAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: /Products/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/LowStock
        public async Task<IActionResult> LowStock()
        {
            var products = await _api.GetProductsAsync();
            var suppliers = await _api.GetSuppliersAsync();

            var supplierMap = suppliers.ToDictionary(s => s.Id, s => s.Name);

            // attach SupplierName to each product
            foreach (var product in products)
            {
                if (supplierMap.TryGetValue(product.SupplierId, out var supplierName))
                    product.SupplierName = supplierName;
                else
                    product.SupplierName = "Unknown Supplier";
            }

            // now filter low stock
            var lowStockProducts = products
                .Where(p => p.QuantityOnHand <= p.ReorderLevel)
                .ToList();

            return View(lowStockProducts);
        }
    }
}

