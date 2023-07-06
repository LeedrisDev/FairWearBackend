namespace BrandAndProductDatabase.Service.Models.Dto;

/// <summary>
/// Class representing a ProductScore in the business.
/// </summary>
public class ProductScoreDto
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