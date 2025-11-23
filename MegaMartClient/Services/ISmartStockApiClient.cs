using MegaMartClient.Models.Dto;

namespace MegaMartClient.Services
{
    public interface ISmartStockApiClient
    {
        // PRODUCTS
        Task<IEnumerable<ProductReadDto>> GetProductsAsync();
        Task<ProductReadDto?> GetProductAsync(int id);
        Task CreateProductAsync(ProductCreateDto dto);
        Task UpdateProductAsync(int id, ProductUpdateDto dto);
        Task PatchProductQuantityAsync(int id, int newQoH);

        Task DeleteProductAsync(int id);

        // SUPPLIERS
        Task<IEnumerable<SupplierReadDto>> GetSuppliersAsync();
        Task<SupplierReadDto?> GetSupplierAsync(int id);
        Task CreateSupplierAsync(SupplierCreateDto dto);
        Task UpdateSupplierAsync(int id, SupplierUpdateDto dto);
        Task PatchSupplierContactAsync(int id, string phone);
        Task DeleteSupplierAsync(int id);

        // PURCHASE ORDERS
        Task<IEnumerable<PurchaseOrderReadDto>> GetPurchaseOrdersAsync();
        Task<PurchaseOrderReadDto?> GetPurchaseOrderAsync(int id);
        Task CreatePurchaseOrderAsync(PurchaseOrderCreateDto dto);
        Task UpdatePurchaseOrderAsync(int id, PurchaseOrderUpdateDto dto);
        Task PatchPurchaseOrderStatusAsync(int id, string newStatus);
        Task DeletePurchaseOrderAsync(int id);
    }
}
