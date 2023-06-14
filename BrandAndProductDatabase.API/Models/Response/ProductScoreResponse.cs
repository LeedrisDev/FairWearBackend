namespace BrandAndProductDatabase.API.Models.Response;

/// <summary>
/// Class representing a ProductScore
/// </summary>
public class ProductScoreResponse
{
    /// <summary>
    /// The moral score of the Product.
    /// </summary>
    public int Moral { get; set; }
    
    /// <summary>
    /// The animal impact score of the Product.
    /// </summary>
    public int Animal { get; set; }
    
    /// <summary>
    /// The environmental impact score of the Product.
    /// </summary>
    public int Environmental { get; set; }
}