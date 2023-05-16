namespace FairWearGateway.API.Models.ServiceResponse;

/// <summary>
/// Model representing a brand response from the BrandAndProductDatabase.API.
/// </summary>
public class BrandServiceResponse
{
    /// <summary>The Id of the Brand.</summary>
    public int Id { get; set; }

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
}