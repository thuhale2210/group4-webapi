using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            _httpClient.BaseAddress = new System.Uri(opt.BaseUrl);

            if (!string.IsNullOrEmpty(opt.ApiKey))
            {
                // For Apigee / x-api-key protected APIs
                _httpClient.DefaultRequestHeaders.Add("x-api-key", opt.ApiKey);
            }
        }

        // ===================== PRODUCTS =====================

        public async Task<IEnumerable<ProductReadDto>> GetProductsAsync()
        {
            var result = await _httpClient
                .GetFromJsonAsync<IEnumerable<ProductReadDto>>("/api/Products");

            return result ?? new List<ProductReadDto>();
        }

        public async Task<ProductReadDto?> GetProductAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Products/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductReadDto>();
        }

        public async Task CreateProductAsync(ProductCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Products", dto);
            response.EnsureSuccessStatusCode();
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

        // ===================== SUPPLIERS =====================

        public async Task<IEnumerable<SupplierReadDto>> GetSuppliersAsync()
        {
            var result = await _httpClient
                .GetFromJsonAsync<IEnumerable<SupplierReadDto>>("/api/Suppliers");

            return result ?? new List<SupplierReadDto>();
        }

        public async Task<SupplierReadDto?> GetSupplierAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Suppliers/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SupplierReadDto>();
        }

        public async Task CreateSupplierAsync(SupplierCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Suppliers", dto);
            response.EnsureSuccessStatusCode();
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

        // ===================== PURCHASE ORDERS =====================

        public async Task<IEnumerable<PurchaseOrderReadDto>> GetPurchaseOrdersAsync()
        {
            var result = await _httpClient
                .GetFromJsonAsync<IEnumerable<PurchaseOrderReadDto>>("/api/PurchaseOrders");

            return result ?? new List<PurchaseOrderReadDto>();
        }

        public async Task<PurchaseOrderReadDto?> GetPurchaseOrderAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/PurchaseOrders/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PurchaseOrderReadDto>();
        }

        public async Task CreatePurchaseOrderAsync(PurchaseOrderCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/PurchaseOrders", dto);
            response.EnsureSuccessStatusCode();
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
