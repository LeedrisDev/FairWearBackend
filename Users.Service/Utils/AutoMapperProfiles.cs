using AutoMapper;
using Users.Service.Models.Dto;
using Users.Service.Models.Entity;

namespace Users.Service.Utils;

/// <summary>AutoMapper profiles, class representing the models to be mapped to each other.</summary>
public class AutoMapperProfiles : Profile
{
    /// <summary>Constructor for AutoMapperProfiles</summary>
    public AutoMapperProfiles()
    {
        // Entity to Dto
        CreateMap<ProductEntity, ProductDto>();
        CreateMap<UserEntity, UserDto>();
        CreateMap<UserExperienceEntity, UserExperienceDto>();
        CreateMap<UserProductHistoryEntity, UserProductHistoryDto>();

        // Dto to Entity
        CreateMap<ProductDto, ProductEntity>();
        CreateMap<UserDto, UserEntity>();
        CreateMap<UserExperienceDto, UserExperienceEntity>();
        CreateMap<UserProductHistoryDto, UserProductHistoryEntity>();

        // Dto to Response


        // Response to Dto
    }
}