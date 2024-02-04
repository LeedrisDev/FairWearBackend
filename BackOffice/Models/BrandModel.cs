using System.ComponentModel.DataAnnotations;

namespace BackOffice.Models;

/// <summary>Represents a brand with associated properties.</summary>
public class BrandModel : IObjectWithId
{
    /// <summary>Gets or sets the unique identifier of the brand.</summary>
    public long Id { get; set; }

    /// <summary>Gets or sets the name of the brand.</summary>
    public string Name { get; set; } = null!;

    /// <summary>Gets or sets the country of origin for the brand.</summary>
    public string Country { get; set; } = null!;

    /// <summary>Gets or sets the environmental rating of the brand.</summary>
    [Range(0, 5, ErrorMessage = "Value must be between 0 and 5")]
    [Display(Name = "Environmental rating")]
    public int EnvironmentRating { get; set; }

    /// <summary>Gets or sets the human rating of the brand.</summary>
    [Range(0, 5, ErrorMessage = "Value must be between 0 and 5")]
    [Display(Name = "Human rating")]
    public int PeopleRating { get; set; }

    /// <summary>Gets or sets the animal rating of the brand.</summary>
    [Range(0, 5, ErrorMessage = "Value must be between 0 and 5")]
    [Display(Name = "Animal rating")]
    public int AnimalRating { get; set; }

    /// <summary>Gets or sets the description of the brand's rating.</summary>
    public string RatingDescription { get; set; } = null!;

    /// <summary>Gets or sets the categories associated with the brand.</summary>
    public List<string> Categories { get; set; } = new List<string>();

    /// <summary>Gets or sets the product ranges associated with the brand.</summary>
    public List<string> Ranges { get; set; } = new List<string>();

    /// <inheritdoc/>
    public override string ToString()
    {
        return "{\n" + $"    Id: {Id}\n" + $"    Name: {Name}\n" + $"    Country: {Country}\n" +
               $"    EnvironmentRating: {EnvironmentRating}\n" + $"    PeopleRating: {PeopleRating}\n" +
               $"    AnimalRating: {AnimalRating}\n" + $"    RatingDescription: {RatingDescription}\n" +
               $"    Categories: {string.Join(", ", Categories)}\n" + $"    Ranges: {string.Join(", ", Ranges)}\n" +
               "}";
    }
}
