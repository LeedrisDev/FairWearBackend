using BackOffice.Models;

namespace BackOffice.DataAccess.Entities;

/// <summary>Class representing a Product in database.</summary>
public class ProductEntity : IObjectWithId
{
    /// <summary>The Id of the Product.</summary>
    public long Id { get; set; }
    
    /// <summary>The UpcCode of the Product.</summary>
    public string UpcCode { get; set; } = null!;

    /// <summary>The Name of the Product.</summary>
    public string Name { get; set; } = null!;

    /// <summary>The category of the Product.</summary>
    public string? Category { get; set; }

    /// <summary>The Ranges of the Product.</summary>
    /// <remarks>TODO : For IEnumerable should use hasconversion in dbcontext </remarks>
    public List<string>? Ranges { get; set; }

    /// <summary>The BrandId of the Product.</summary>
    public long BrandId { get; set; }

    /// <summary>The <see cref="BrandEntity"/> of the Product.</summary>
    public virtual BrandEntity BrandEntity { get; set; } = null!;
}