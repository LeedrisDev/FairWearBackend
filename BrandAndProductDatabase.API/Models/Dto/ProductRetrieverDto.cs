namespace BrandAndProductDatabase.API.Models.Dto;

/// <summary>
/// DTO for retrieving a product from the productRetriever microservice.
/// </summary>
public class ProductRetrieverDto
{
    /// <summary>UPC code of the product</summary>
    public string UpcCode { get; set; } = null!;

    /// <summary>Name of the product</summary>
    public string Name { get; set; } = null!;

    /// <summary>Brand name of the product</summary>
    public string BrandName { get; set; } = null!;

    /// <summary>Category of the product</summary>
    public string Category { get; set; } = "";

    /// <summary>Range of the product</summary>
    public IEnumerable<string> Ranges { get; set; } = new List<string>();
}