using System;
using System.Collections.Generic;
using MegaMartClient.Models.Dto;   // ⬅️ add this

namespace MegaMartClient.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int LowStockItems { get; set; }
        public int SupplierCount { get; set; }
        public int OpenPurchaseOrders { get; set; }

        public List<CategoryStockItem> CategoryBreakdown { get; set; } = new();
        public List<DailyPoStat> Last7Days { get; set; } = new();

        // ⭐ NEW: for Low Stock Products chart
        public List<ProductReadDto> LowStockProducts { get; set; } = new();
    }

    public class CategoryStockItem
    {
        public string Category { get; set; } = string.Empty;
        public int Percentage { get; set; }   // 0–100
    }

    public class DailyPoStat
    {
        public DateOnly Date { get; set; }
        public int PoCount { get; set; }
        public string StatusLabel { get; set; } = string.Empty;
    }
}
