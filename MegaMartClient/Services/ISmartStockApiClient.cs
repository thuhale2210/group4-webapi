using MegaMartClient.Models.Dto;

namespace MegaMartClient.Services
{
    public interface ISmartStockApiClient
    {
        // PRODUCTS
        Task<IEnumerable<ProductReadDto>> GetProductsAsync();
        Task<ProductReadDto?> GetProductAsync(int id);
        Task<ProductReadDto> CreateProductAsync(ProductCreateDto dto);
        Task UpdateProductAsync(int id, ProductUpdateDto dto);
        Task DeleteProductAsync(int id);

        // SUPPLIERS
        Task<IEnumerable<SupplierReadDto>> GetSuppliersAsync();
        Task<SupplierReadDto?> GetSupplierAsync(int id);
        Task<SupplierReadDto> CreateSupplierAsync(SupplierCreateDto dto);
        Task UpdateSupplierAsync(int id, SupplierUpdateDto dto);
        Task DeleteSupplierAsync(int id);

        // PURCHASE ORDERS
        Task<IEnumerable<PurchaseOrderReadDto>> GetPurchaseOrdersAsync();
        Task<PurchaseOrderReadDto?> GetPurchaseOrderAsync(int id);
        Task<PurchaseOrderReadDto> CreatePurchaseOrderAsync(PurchaseOrderCreateDto dto);
        Task UpdatePurchaseOrderAsync(int id, PurchaseOrderUpdateDto dto);
        Task DeletePurchaseOrderAsync(int id);
    }
}
