using System;
using System.Collections.Generic;

namespace BrandAndProductDatabase.API.Models;

public partial class Product
{
    public int Id { get; set; }

    public string UpcCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public string[]? Ranges { get; set; }

    public int BrandId { get; set; }

    public virtual Brand Brand { get; set; } = null!;
}
