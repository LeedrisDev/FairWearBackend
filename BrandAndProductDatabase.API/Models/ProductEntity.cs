namespace BrandAndProductDatabase.API.Models;

public class ProductEntity
{
    public int Id { get; set; }

    public string UpcCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public string[]? Ranges { get; set; }

    public int BrandId { get; set; }

    public virtual BrandEntity BrandEntity { get; set; } = null!;
}
