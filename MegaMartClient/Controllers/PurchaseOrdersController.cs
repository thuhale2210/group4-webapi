using MegaMartClient.Models.Dto;
using MegaMartClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace MegaMartClient.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private readonly ISmartStockApiClient _api;

        public PurchaseOrdersController(ISmartStockApiClient api)
        {
            _api = api;
        }

        // GET: PurchaseOrders
        public async Task<IActionResult> Index()
        {
            var orders = await _api.GetPurchaseOrdersAsync();
            return View(orders); // IEnumerable<PurchaseOrderReadDto>
        }

        // GET: PurchaseOrders/Create
        public IActionResult Create()
        {
            // Default status "Pending", default date today (for the form)
            var dto = new PurchaseOrderCreateDto(
                ProductId: 0,
                SupplierId: 0,
                Quantity: 1
            );

            return View(dto);
        }

        // POST: PurchaseOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _api.CreatePurchaseOrderAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: PurchaseOrders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _api.GetPurchaseOrderAsync(id);
            if (order is null) return NotFound();

            var updateDto = new PurchaseOrderUpdateDto(
                ProductId: order.ProductId,
                SupplierId: order.SupplierId,
                Quantity: order.Quantity,
                Status: order.Status
            );

            return View(updateDto);
        }

        // POST: PurchaseOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseOrderUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _api.UpdatePurchaseOrderAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: PurchaseOrders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _api.GetPurchaseOrderAsync(id);
            if (order is null) return NotFound();

            return View(order); // PurchaseOrderReadDto
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeletePurchaseOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
