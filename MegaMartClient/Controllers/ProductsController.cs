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

        // ------------------------------
        // GET: /Products
        // ------------------------------
        public async Task<IActionResult> Index()
        {
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

        // ------------------------------
        // GET: /Products/Details/5
        // ------------------------------
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

        // ------------------------------
        // GET: /Products/Create
        // ------------------------------
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

        // ------------------------------
        // POST: /Products/Create
        // ------------------------------
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

        // ------------------------------
        // GET: /Products/Edit/5
        // ------------------------------
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

        // ------------------------------
        // POST: /Products/Edit/5
        // ------------------------------
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


        // ------------------------------
        // POST: /Products/Delete/5
        // ------------------------------
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // ------------------------------
        // GET: /Products/LowStock
        // ------------------------------
        public async Task<IActionResult> LowStock()
        {
            var products = await _api.GetProductsAsync();
            var lowStockProducts = products
                .Where(p => p.QuantityOnHand <= p.ReorderLevel)
                .ToList();

            return View(lowStockProducts);
        }
    }
}

