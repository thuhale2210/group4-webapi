using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MegaMartClient.Models.Dto;
using MegaMartClient.Services;

namespace MegaMartClient.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISmartStockApiClient _api;

        public SuppliersController(ISmartStockApiClient api)
        {
            _api = api;
        }

        // GET: /Suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _api.GetSuppliersAsync();
            return View(suppliers);
        }

        // GET: /Suppliers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _api.GetSupplierAsync(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        // GET: /Suppliers/Create
        public IActionResult Create()
        {
            return View(new SupplierCreateDto());
        }

        // POST: /Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _api.CreateSupplierAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Suppliers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _api.GetSupplierAsync(id);
            if (supplier == null) return NotFound();

            var vm = new SupplierUpdateDto
            {
                Name = supplier.Name,
                ContactEmail = supplier.ContactEmail,
                Phone = supplier.Phone
            };

            ViewBag.Id = id;
            return View(vm);
        }

        // POST: /Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierUpdateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _api.UpdateSupplierAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Suppliers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _api.GetSupplierAsync(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        // POST: /Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeleteSupplierAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}



//using Microsoft.AspNetCore.Mvc;
//using MegaMartClient.Models.Dto;

//namespace MegaMartClient.Controllers
//{
//    public class SuppliersController : Controller
//    {
//        // TEMPORARY MOCK DATA (swap later for API calls)
//        private static readonly List<SupplierReadDto> mockSuppliers = new()
//        {
//            new SupplierReadDto
//            {
//                Id = 1,
//                Name = "Fresh Farms Ltd.",
//                ContactEmail = "contact@freshfarms.com",
//                Phone = "416-555-1234"
//            },
//            new SupplierReadDto
//            {
//                Id = 2,
//                Name = "Organic Harvest Co.",
//                ContactEmail = "sales@organicharvest.com",
//                Phone = "647-555-9876"
//            },
//            new SupplierReadDto
//            {
//                Id = 3,
//                Name = "DairyPure Suppliers",
//                ContactEmail = "support@dairypure.ca",
//                Phone = "905-555-4444"
//            }
//        };

//        // GET: /Suppliers
//        public IActionResult Index()
//        {
//            return View(mockSuppliers);
//        }

//        // GET: /Suppliers/Details/5
//        public IActionResult Details(int id)
//        {
//            var supplier = mockSuppliers.FirstOrDefault(s => s.Id == id);
//            if (supplier == null) return NotFound();

//            return View(supplier);
//        }

//        // GET: /Suppliers/Create
//        public IActionResult Create()
//        {
//            var vm = new SupplierReadDto();
//            return View(vm);
//        }

//        // POST: /Suppliers/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(SupplierReadDto dto)
//        {
//            if (!ModelState.IsValid)
//                return View(dto);

//            var nextId = mockSuppliers.Any() ? mockSuppliers.Max(s => s.Id) + 1 : 1;
//            dto.Id = nextId;

//            mockSuppliers.Add(dto);

//            return RedirectToAction(nameof(Index));
//        }

//        // GET: /Suppliers/Edit/5
//        public IActionResult Edit(int id)
//        {
//            var supplier = mockSuppliers.FirstOrDefault(s => s.Id == id);
//            if (supplier == null) return NotFound();

//            // You can pass the same DTO directly
//            return View(supplier);
//        }

//        // POST: /Suppliers/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Edit(int id, SupplierReadDto dto)
//        {
//            if (!ModelState.IsValid)
//                return View(dto);

//            var supplier = mockSuppliers.FirstOrDefault(s => s.Id == id);
//            if (supplier == null) return NotFound();

//            supplier.Name = dto.Name;
//            supplier.ContactEmail = dto.ContactEmail;
//            supplier.Phone = dto.Phone;

//            return RedirectToAction(nameof(Index));
//        }

//        // GET: /Suppliers/Delete/5
//        public IActionResult Delete(int id)
//        {
//            var supplier = mockSuppliers.FirstOrDefault(s => s.Id == id);
//            if (supplier == null) return NotFound();

//            return View(supplier);
//        }

//        // POST: /Suppliers/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            var supplier = mockSuppliers.FirstOrDefault(s => s.Id == id);
//            if (supplier != null)
//            {
//                mockSuppliers.Remove(supplier);
//            }

//            return RedirectToAction(nameof(Index));
//        }
//    }
//}


