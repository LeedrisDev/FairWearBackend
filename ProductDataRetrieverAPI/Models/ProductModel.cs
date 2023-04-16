namespace ProductDataRetrieverAPI.Models;

public class ProductModel
{
    public string? UpcCode { get; set; }
    public string? Name { get; set; }
    public string? BrandName { get; set; }
    public string? Category { get; set; }
    public string[]? Ranges { get; set; }
}