using AutoMapper;
using BackOffice.DataAccess.Entities;
using BackOffice.Models;

namespace BackOffice.DataAccess;

/// <summary>Defines AutoMapper profiles for mapping between Models and Entities.</summary>
public class AutoMapperProfiles : Profile
{
    /// <summary>
    /// Initializes AutoMapper profiles for mappings between <see cref="BrandModel"/> and <see cref="BrandEntity"/>.
    /// </summary>
    public AutoMapperProfiles()
    {
        // Model to Entity
        CreateMap<BrandModel, BrandEntity>()
            .IgnoreId();
        
        // Entity to Model
        CreateMap<BrandEntity, BrandModel>();
    }
}

/// <summary>Provides extension methods for AutoMapper configuration to ignore certain members during mapping.</summary>
public static class AutomapperIgnoreAttributes
{
    /// <summary>
    /// Ignores the 'Id' member during mapping from <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The source type for mapping.</typeparam>
    /// <typeparam name="TDestination">
    /// The destination type for mapping, which should implement <see cref="IObjectWithId"/>.
    /// </typeparam>
    /// <param name="map">The mapping expression to configure.</param>
    /// <returns>The updated mapping expression.</returns>
    public static void IgnoreId<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map) where TDestination : IObjectWithId
    {
        map.ForMember(entity => entity.Id, opt => opt.Ignore());
    }
}
