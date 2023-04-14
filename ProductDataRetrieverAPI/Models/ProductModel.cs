namespace ProductDataRetrieverAPI.Models;

public class ProductModel
{
    public string? Name { get; set; }
    public string? BrandName { get; set; }
    public string[]? Categories { get; set; }
    public string[]? Ranges { get; set; }
}