namespace GoodOnYouScrapperAPI.Models;

public class BrandModel
{
     public string? Name { get; set; }
     public string? Country { get; set; }
     public int EnvironmentRating { get; set; }
     public int PeopleRating { get; set; }
     public int AnimalRating { get; set; }
     public string? RatingDescription { get; set; }
     public string[]? Categories { get; set; }
     public string[]? Ranges { get; set; }

}