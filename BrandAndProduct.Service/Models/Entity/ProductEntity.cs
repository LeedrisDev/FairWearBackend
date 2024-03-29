﻿namespace BrandAndProduct.Service.Models.Entity;

/// <summary>Class representing a Product in database.</summary>
public class ProductEntity : IObjectWithId
{
    /// <summary>The Id of the Product.</summary>
    public int Id { get; set; }
    
    /// <summary>The UpcCode of the Product.</summary>
    public string UpcCode { get; set; } = null!;

    /// <summary>The Name of the Product.</summary>
    public string Name { get; set; } = null!;

    /// <summary>The category of the Product.</summary>
    public string Category { get; set; } = null!;
    
    /// <summary>The Color of the Product.</summary>
    public string Color { get; set; } = null!;

    /// <summary>The Ranges of the Product.</summary>
    /// <remarks>TODO : For IEnumerable should use hasconversion in dbcontext </remarks>
    public List<string>? Ranges { get; set; }

    /// <summary>The BrandId of the Product.</summary>
    public int BrandId { get; set; }

    /// <summary>The <see cref="BrandEntity"/> of the Product.</summary>
    public virtual BrandEntity BrandEntity { get; set; } = null!;
}