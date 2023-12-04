using AutoMapper;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Models.Entity;
using BrandAndProduct.Service.Protos;
using GoodOnYouScrapper.Service.Protos;

namespace BrandAndProduct.Service.Utils;

/// <summary>AutoMapper profiles, class representing the models to be mapped to each other.</summary>
public class AutoMapperProfiles : Profile
{
    /// <summary>Constructor for AutoMapperProfiles</summary>
    public AutoMapperProfiles()
    {
        // Entity to Dto
        CreateMap<BrandEntity, BrandDto>();
        CreateMap<ProductEntity, ProductDto>();
        CreateMap<IntegrationEventEntity, IntegrationEventDto>();

        // Dto to Entity
        CreateMap<BrandDto, BrandEntity>();
        CreateMap<ProductDto, ProductEntity>();
        CreateMap<IntegrationEventDto, IntegrationEventEntity>();

        // Dto to Response
        CreateMap<BrandDto, BrandResponse>();
        CreateMap<ProductDto, ProductResponse>();
        CreateMap<ProductInformationDto, ProductInformationResponse>();
        CreateMap<ProductCompositionDto, ProductCompositionResponse>();
        CreateMap<ProductScoreDto, ProductScoreResponse>();

        // Response to Dto
        CreateMap<BrandResponse, BrandDto>();
        CreateMap<BrandRequest, BrandDto>();
        CreateMap<BrandScrapperResponse, BrandDto>();
        CreateMap<ProductResponse, ProductDto>();
        CreateMap<ProductRequest, ProductDto>();


        // CreateMap<ProductInformationResponse, ProductInformationDto>();
        // CreateMap<ProductCompositionResponse, ProductCompositionDto>();
        // CreateMap<ProductScoreResponse, ProductScoreDto>();
    }
}