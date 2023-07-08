using BrandAndProductDatabase.Service.Models.Entity;

namespace BrandAndProductDatabase.Service.Models.Dto;

/// <summary>Class representing a Brand in the business.</summary>
public class BrandDto : IObjectWithId
{
    /// <summary>The Name of the Brand.</summary>
    public string Name { get; set; } = null!;

    /// <summary>The Country of the Brand.</summary>
    public string Country { get; set; } = null!;

    /// <summary>The EnvironmentRating of the Brand.</summary>
    public int EnvironmentRating { get; set; }

    /// <summary>The PeopleRating of the Brand.</summary>
    public int PeopleRating { get; set; }

    /// <summary>The AnimalRating of the Brand.</summary>
    public int AnimalRating { get; set; }

    /// <summary>The RatingDescription of the Brand.</summary>
    public string RatingDescription { get; set; } = null!;

    /// <summary>The Categories of the Brand.</summary>
    public List<string> Categories { get; set; } = null!;

    /// <summary>The Ranges of the Brand.</summary>
    public List<string> Ranges { get; set; } = null!;

    /// <summary>The <see cref="ProductEntity"/>s of the Brand.</summary>
    public virtual IEnumerable<ProductEntity> Products { get; set; } = new List<ProductEntity>();

    /// <summary>The Id of the Brand.</summary>
    public int Id { get; set; }
}