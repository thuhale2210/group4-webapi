using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using MegaMartClient.Models;
using MegaMartClient.Models.Dto;

namespace MegaMartClient.Services
{
    public class SmartStockApiClient : ISmartStockApiClient
    {
        private readonly HttpClient _httpClient;

        public SmartStockApiClient(HttpClient httpClient, IOptions<SmartStockApiOptions> options)
        {
            _httpClient = httpClient;
            var opt = options.Value;
            _httpClient.BaseAddress = new Uri(opt.BaseUrl);

            if (!string.IsNullOrEmpty(opt.ApiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("x-api-key", opt.ApiKey);
            }
        }

        // ===== Products =====

        public async Task<IEnumerable<ProductReadDto>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("/api/Products"); // or /api/Product if that's your route
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<ProductReadDto>>()
                   ?? Enumerable.Empty<ProductReadDto>();
        }

        public async Task<ProductReadDto?> GetProductAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Products/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductReadDto>();
        }

        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Products", dto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductReadDto>()
                   ?? throw new InvalidOperationException("API did not return created product.");
        }

        public async Task UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Products/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Products/{id}");
            response.EnsureSuccessStatusCode();
        }

        // ===== Suppliers =====

        public async Task<IEnumerable<SupplierReadDto>> GetSuppliersAsync()
        {
            var response = await _httpClient.GetAsync("/api/Suppliers");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<SupplierReadDto>>()
                   ?? Enumerable.Empty<SupplierReadDto>();
        }

        public async Task<SupplierReadDto?> GetSupplierAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Suppliers/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SupplierReadDto>();
        }

        public async Task<SupplierReadDto> CreateSupplierAsync(SupplierCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Suppliers", dto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<SupplierReadDto>()
                   ?? throw new InvalidOperationException("API did not return created supplier.");
        }

        public async Task UpdateSupplierAsync(int id, SupplierUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Suppliers/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Suppliers/{id}");
            response.EnsureSuccessStatusCode();
        }

        // ===== Purchase Orders =====

        public async Task<IEnumerable<PurchaseOrderReadDto>> GetPurchaseOrdersAsync()
        {
            var response = await _httpClient.GetAsync("/api/PurchaseOrders");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<PurchaseOrderReadDto>>()
                   ?? Enumerable.Empty<PurchaseOrderReadDto>();
        }

        public async Task<PurchaseOrderReadDto?> GetPurchaseOrderAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/PurchaseOrders/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PurchaseOrderReadDto>();
        }

        public async Task<PurchaseOrderReadDto> CreatePurchaseOrderAsync(PurchaseOrderCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/PurchaseOrders", dto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PurchaseOrderReadDto>()
                   ?? throw new InvalidOperationException("API did not return created purchase order.");
        }

        public async Task UpdatePurchaseOrderAsync(int id, PurchaseOrderUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/PurchaseOrders/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/PurchaseOrders/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
