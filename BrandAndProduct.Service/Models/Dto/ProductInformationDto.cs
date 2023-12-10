namespace BrandAndProduct.Service.Models.Dto;

/// <summary>
/// Class representing a ProductInformation in the business.
/// </summary>
public class ProductInformationDto
{
    public int Id { get; set; }
    /// <summary>
    /// The Name of the Product.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The country of production the Product.
    /// </summary>
    public string Country { get; set; } = null!;


    /// <summary>
    /// The image url of the product
    /// </summary>
    public string Image { get; set; } = null!;

    /// <summary>
    /// The global score of the product
    /// </summary>
    public int GlobalScore { get; set; }

    /// <summary>
    /// The scores of the product
    /// </summary>
    public ProductScoreDto Scores { get; set; } = null!;

    /// <summary>
    /// The product composition of the product
    /// </summary>
    public IEnumerable<ProductCompositionDto> Composition { get; set; } = null!;

    /// <summary>
    /// The BrandId of the Product.
    /// </summary>
    public string Brand { get; set; } = null!;
}