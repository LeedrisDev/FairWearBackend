using AutoMapper;
using BrandAndProductDatabase.Service.Models.Dto;
using BrandAndProductDatabase.Service.Models.Entity;
using BrandAndProductDatabase.Service.Protos;

namespace BrandAndProductDatabase.Service.Utils;

/// <summary>AutoMapper profiles, class representing the models to be mapped to each other.</summary>
public class AutoMapperProfiles : Profile
{
    /// <summary>Constructor for AutoMapperProfiles</summary>
    public AutoMapperProfiles()
    {
        // Entity to Dto
        CreateMap<BrandEntity, BrandDto>();
        CreateMap<ProductEntity, ProductDto>();

        // Dto to Entity
        CreateMap<BrandDto, BrandEntity>();
        CreateMap<ProductDto, ProductEntity>();

        // Dto to Response
        CreateMap<BrandDto, BrandResponse>();
        // CreateMap<ProductDto, ProductResponse>();
        // CreateMap<ProductInformationDto, ProductInformationResponse>();
        // CreateMap<ProductCompositionDto, ProductCompositionResponse>();
        // CreateMap<ProductScoreDto, ProductScoreResponse>();

        // Response to Dto
        CreateMap<BrandResponse, BrandDto>();
        CreateMap<BrandRequest, BrandDto>();
        // CreateMap<ProductResponse, ProductDto>();


        // CreateMap<ProductInformationResponse, ProductInformationDto>();
        // CreateMap<ProductCompositionResponse, ProductCompositionDto>();
        // CreateMap<ProductScoreResponse, ProductScoreDto>();
    }
}