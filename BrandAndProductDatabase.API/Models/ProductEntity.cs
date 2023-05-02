﻿namespace BrandAndProductDatabase.API.Models;

/// <summary>Class representing a Product in database.</summary>
public class ProductEntity
{
    /// <summary>The Id of the Product.</summary>
    public int Id { get; set; }

    /// <summary>The UpcCode of the Product.</summary>
    public string UpcCode { get; set; } = null!;

    /// <summary>The Name of the Product.</summary>
    public string Name { get; set; } = null!;

    /// <summary>The category of the Product.</summary>
    public string? Category { get; set; }
    
    /// <summary>The Ranges of the Product.</summary>
    public IEnumerable<string>? Ranges { get; set; }

    /// <summary>The BrandId of the Product.</summary>
    public int BrandId { get; set; }

    /// <summary>The <see cref="BrandEntity"/> of the Product.</summary>
    public virtual BrandEntity BrandEntity { get; set; } = null!;
}
