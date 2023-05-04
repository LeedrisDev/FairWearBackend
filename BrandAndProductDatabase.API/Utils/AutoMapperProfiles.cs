using AutoMapper;

namespace BrandAndProductDatabase.API.Utils;

/// <summary>AutoMapper profiles, class representing the models to be mapped to each other.</summary>
public class AutoMapperProfiles : Profile
{
    /// <summary>Constructor for AutoMapperProfiles</summary>
    public AutoMapperProfiles()
    {
        CreateMap<Models.Entity.BrandEntity, Models.Dto.BrandDto>();
        CreateMap<Models.Dto.BrandDto, Models.Entity.BrandEntity>();

        CreateMap<Models.Entity.ProductEntity, Models.Dto.ProductDto>();
        CreateMap<Models.Dto.ProductDto, Models.Entity.ProductEntity>();
    }
}