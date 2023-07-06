namespace BrandAndProductDatabase.Service.Models.Response;

/// <summary>
/// Class representing a ProductComposition
/// </summary>
public class ProductCompositionResponse
{
    /// <summary>
    /// Percentage of the type of fabrics in the Product.
    /// </summary>
    public int Percentage { get; set; }

    /// <summary>
    /// Type of fabric in the Product.
    /// </summary>
    public string Component { get; set; } = null!;
}