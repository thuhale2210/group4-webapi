using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MegaMartClient.Models.ViewModels
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }

        public int SupplierId { get; set; }

        public string? ImageUrl { get; set; }

        // DROPDOWN OPTIONS
        public IEnumerable<SelectListItem> SupplierOptions { get; set; }
            = new List<SelectListItem>();
    }
}
