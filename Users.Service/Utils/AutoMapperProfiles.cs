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

        // Dto to proto object
        CreateMap<UserDto, User>();
        CreateMap<UserProductHistoryDto, UserProductHistory>();
        CreateMap<UserExperienceDto, UserExperience>();
        CreateMap<ProductDto, Product>();

        // Proto object to Dto
        CreateMap<User, UserDto>();
        CreateMap<UserProductHistory, UserProductHistoryDto>();
        CreateMap<UserExperience, UserExperienceDto>();
        CreateMap<Product, ProductDto>();
    }
}