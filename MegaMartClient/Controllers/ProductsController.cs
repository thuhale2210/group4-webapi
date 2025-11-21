using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MegaMartClient.Models.Dto;
using MegaMartClient.Services;

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

            return View(product);
        }

        // ------------------------------
        // GET: /Products/Create
        // ------------------------------
        public IActionResult Create()
        {
            var dto = new ProductCreateDto();
            return View(dto);
        }

        // ------------------------------
        // POST: /Products/Create
        // ------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

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

            var dto = new ProductUpdateDto
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
            return View(dto);
        }

        // ------------------------------
        // POST: /Products/Edit/5
        // ------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _api.UpdateProductAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        // ------------------------------
        // GET: /Products/Delete/5
        // ------------------------------
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _api.GetProductAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
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


//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.AspNetCore.Mvc;
//using MegaMartClient.Models.Dto;

//namespace MegaMartClient.Controllers
//{
//    public class ProductsController : Controller
//    {
//        // Mock product list
//        private static readonly List<ProductReadDto> _mockProducts = new()
//        {
//            new ProductReadDto
//            {
//                Id = 1,
//                Name = "Test Apples",
//                Category = "Fruit",
//                UnitPrice = 2.99m,
//                QuantityOnHand = 20,
//                ReorderLevel = 5,
//                SupplierId = 1,
//                ImageUrl = "https://www.vhv.rs/dpng/d/524-5240049_transparent-upset-emoji-png-apple-emoji-transparent-png.png"
//            },
//            new ProductReadDto
//            {
//                Id = 2,
//                Name = "Organic Milk",
//                Category = "Dairy",
//                UnitPrice = 4.49m,
//                QuantityOnHand = 15,
//                ReorderLevel = 3,
//                SupplierId = 2,
//                ImageUrl = "https://static.vecteezy.com/.../milk.png"
//            }
//        };

//        // ------------------------------
//        // GET: /Products
//        // ------------------------------
//        public IActionResult Index()
//        {
//            return View(_mockProducts);
//        }

//        // ------------------------------
//        // GET: /Products/Details/5
//        // ------------------------------
//        public IActionResult Details(int id)
//        {
//            var product = _mockProducts.FirstOrDefault(p => p.Id == id);
//            if (product == null)
//                return NotFound();

//            return View(product);
//        }

//        // ------------------------------
//        // GET: /Products/Edit/5
//        // ------------------------------
//        public IActionResult Edit(int id)
//        {
//            var product = _mockProducts.FirstOrDefault(p => p.Id == id);
//            if (product == null)
//                return NotFound();

//            var dto = new ProductUpdateDto
//            {
//                Name = product.Name,
//                Category = product.Category,
//                UnitPrice = product.UnitPrice,
//                QuantityOnHand = product.QuantityOnHand,
//                ReorderLevel = product.ReorderLevel,
//                SupplierId = product.SupplierId,
//                ImageUrl = product.ImageUrl
//            };

//            return View(dto);
//        }

//        // ------------------------------
//        // POST: /Products/Edit/5
//        // ------------------------------
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Edit(int id, ProductUpdateDto dto)
//        {
//            if (!ModelState.IsValid)
//                return View(dto);

//            var existing = _mockProducts.FirstOrDefault(p => p.Id == id);
//            if (existing == null)
//                return NotFound();

//            existing.Name = dto.Name;
//            existing.Category = dto.Category;
//            existing.UnitPrice = dto.UnitPrice;
//            existing.QuantityOnHand = dto.QuantityOnHand;
//            existing.ReorderLevel = dto.ReorderLevel;
//            existing.SupplierId = dto.SupplierId;
//            existing.ImageUrl = dto.ImageUrl;

//            return RedirectToAction(nameof(Index));
//        }

//        // ------------------------------
//        // GET: /Products/Delete/5
//        // ------------------------------
//        public IActionResult Delete(int id)
//        {
//            var product = _mockProducts.FirstOrDefault(p => p.Id == id);
//            if (product == null)
//                return NotFound();

//            return View(product);
//        }

//        // ------------------------------
//        // POST: /Products/Delete/5
//        // ------------------------------
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            var product = _mockProducts.FirstOrDefault(p => p.Id == id);
//            if (product != null)
//                _mockProducts.Remove(product);

//            return RedirectToAction(nameof(Index));
//        }

//        // ------------------------------
//        // GET: /Products/LowStock
//        // ------------------------------
//        public IActionResult LowStock()
//        {
//            var lowStockProducts = _mockProducts
//                .Where(p => p.QuantityOnHand <= p.ReorderLevel)
//                .ToList();

//            return View(lowStockProducts);
//        }
//    }
//}
