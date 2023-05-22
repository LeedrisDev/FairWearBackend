namespace BrandAndProductDatabase.API.Models.Response;

public class ProductScanResponse
{
    /// <summary>The Name of the Product.</summary>
    public string name { get; set; } = null!;

    public string country { get; set; } = null!;

    public string image { get; set; } = null!;

    public int globalScore { get; set; }

    public ProductScoreResponse scores { get; set; }

    public ProductCompositionResponse[] composition { get; set; }

    public string[] alternatives { get; set; }

    /// <summary>The BrandId of the Product.</summary>
    public string brand { get; set; }
}