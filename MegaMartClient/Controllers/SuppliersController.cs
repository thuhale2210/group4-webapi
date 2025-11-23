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

        // GET: /Suppliers/Details
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

        // GET: /Suppliers/Edit
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

        // POST: /Suppliers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierUpdateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _api.UpdateSupplierAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        // PATCH: /Suppliers/QuickUpdateContact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickUpdateContact(int id, string phone)
        {
            await _api.PatchSupplierContactAsync(id, phone);
            return RedirectToAction("Details", new { id });
        }

        // GET: /Suppliers/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _api.GetSupplierAsync(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        // POST: /Suppliers/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeleteSupplierAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
