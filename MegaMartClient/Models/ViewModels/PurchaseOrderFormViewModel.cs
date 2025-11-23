using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MegaMartClient.Models.ViewModels
{
    public class PurchaseOrderFormViewModel
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";

        // DROPDOWN OPTIONS
        public IEnumerable<SelectListItem> SupplierOptions { get; set; }
            = new List<SelectListItem>();

        public IEnumerable<SelectListItem> ProductOptions { get; set; }
            = new List<SelectListItem>();

        public string? SelectedProductName { get; set; }
        public string? SelectedProductCategory { get; set; }
        public int? SelectedProductQuantityOnHand { get; set; }
        public int? SelectedProductReorderLevel { get; set; }
        public int? SuggestedQuantity { get; set; }
    }
}
