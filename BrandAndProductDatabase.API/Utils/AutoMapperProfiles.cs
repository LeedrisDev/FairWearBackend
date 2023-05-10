using AutoMapper;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Models.Entity;
using BrandAndProductDatabase.API.Models.Response;

namespace BrandAndProductDatabase.API.Utils;

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
        CreateMap<ProductDto, ProductResponse>();

        
        CreateMap<BrandResponse, BrandDto>();
        CreateMap<ProductResponse, ProductDto>();
    }
}