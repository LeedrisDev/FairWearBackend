namespace GoodOnYouScrapper.API.Models.Response;

/// <summary>Model for the brand</summary>
public class BrandResponse
{
     /// <summary>Id of the brand (to avoid mapping in calling service)</summary>
     public int Id { get; set; } = 0;
     
     /// <summary>Name of the brand</summary>
     public string? Name { get; set; }
     
     /// <summary>Country of the brand</summary>
     public string? Country { get; set; }
     
     /// <summary>Environment rating of the brand</summary>
     public int EnvironmentRating { get; set; }
     
     /// <summary>People rating of the brand</summary>
     public int PeopleRating { get; set; }
     
     /// <summary>Animal rating of the brand</summary>
     public int AnimalRating { get; set; }
     
     /// <summary>Description of the rating</summary>
     public string? RatingDescription { get; set; }
     
     /// <summary>Categories of the brand</summary>
     public IEnumerable<string>? Categories { get; set; }
     
     /// <summary>Ranges of the brand</summary>
     public IEnumerable<string>? Ranges { get; set; }

}