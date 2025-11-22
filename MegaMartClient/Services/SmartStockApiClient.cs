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
                _httpClient.DefaultRequestHeaders.Remove("apikey");
                _httpClient.DefaultRequestHeaders.Add("apikey", opt.ApiKey);
            }
        }

        // ===================== PRODUCTS =====================

        public async Task<IEnumerable<ProductReadDto>> GetProductsAsync()
        {
            var result = await _httpClient
                .GetFromJsonAsync<IEnumerable<ProductReadDto>>("Products");

            return result ?? new List<ProductReadDto>();
        }

        public async Task<ProductReadDto?> GetProductAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Products/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductReadDto>();
        }

        public async Task CreateProductAsync(ProductCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Products", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"Products/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Products/{id}");
            response.EnsureSuccessStatusCode();
        }

        // ===================== SUPPLIERS =====================

        public async Task<IEnumerable<SupplierReadDto>> GetSuppliersAsync()
        {
            var result = await _httpClient
                .GetFromJsonAsync<IEnumerable<SupplierReadDto>>("Suppliers");

            return result ?? new List<SupplierReadDto>();
        }

        public async Task<SupplierReadDto?> GetSupplierAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Suppliers/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SupplierReadDto>();
        }

        public async Task CreateSupplierAsync(SupplierCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Suppliers", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateSupplierAsync(int id, SupplierUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"Suppliers/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Suppliers/{id}");
            response.EnsureSuccessStatusCode();
        }

        // ===================== PURCHASE ORDERS =====================

        public async Task<IEnumerable<PurchaseOrderReadDto>> GetPurchaseOrdersAsync()
        {
            var result = await _httpClient
                .GetFromJsonAsync<IEnumerable<PurchaseOrderReadDto>>("PurchaseOrders");

            return result ?? new List<PurchaseOrderReadDto>();
        }

        public async Task<PurchaseOrderReadDto?> GetPurchaseOrderAsync(int id)
        {
            var response = await _httpClient.GetAsync($"PurchaseOrders/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PurchaseOrderReadDto>();
        }

        public async Task CreatePurchaseOrderAsync(PurchaseOrderCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("PurchaseOrders", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdatePurchaseOrderAsync(int id, PurchaseOrderUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"PurchaseOrders/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"PurchaseOrders/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
