using AutoMapper;
using FreshSourceAPI.DTOs;
using FreshSourceAPI.Entities;

namespace FreshSourceAPI.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();

        // Supplier
        CreateMap<Supplier, SupplierReadDto>();
        CreateMap<SupplierCreateDto, Supplier>();
        CreateMap<SupplierUpdateDto, Supplier>();

        // PurchaseOrder
        CreateMap<PurchaseOrder, PurchaseOrderReadDto>();
        CreateMap<PurchaseOrderCreateDto, PurchaseOrder>();
        CreateMap<PurchaseOrderUpdateDto, PurchaseOrder>();
    }
}
