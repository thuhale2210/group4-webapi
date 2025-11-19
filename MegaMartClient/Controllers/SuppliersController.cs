using MegaMartClient.Models.Dto;
using MegaMartClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace MegaMartClient.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISmartStockApiClient _api;

        public SuppliersController(ISmartStockApiClient api)
        {
            _api = api;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _api.GetSuppliersAsync();
            return View(suppliers); // IEnumerable<SupplierReadDto>
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View(new SupplierCreateDto
            (
                Name: "",
                ContactEmail: "",
                Phone: ""
            ));
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _api.CreateSupplierAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _api.GetSupplierAsync(id);
            if (supplier is null) return NotFound();

            var updateDto = new SupplierUpdateDto(
                supplier.Name,
                supplier.ContactEmail,
                supplier.Phone
            );

            return View(updateDto);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _api.UpdateSupplierAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _api.GetSupplierAsync(id);
            if (supplier is null) return NotFound();

            return View(supplier); // SupplierReadDto
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeleteSupplierAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
