namespace FairWearProductDataRetriever.Service.Models;

/// <summary>Model for a product</summary>
public class ProductModel
{
    /// <summary>UPC code of the product</summary>
    public string UpcCode { get; set; } = "";

    /// <summary>Name of the product</summary>
    public string Name { get; set; } = "";

    /// <summary>Brand name of the product</summary>
    public string BrandName { get; set; } = "";

    /// <summary>Category of the product</summary>
    public string Category { get; set; } = "";

    /// <summary>Range of the product</summary>
    public IEnumerable<string> Ranges { get; set; } = new List<string>();
}