using System;
using System.Collections.Generic;

namespace BrandAndProductDatabase.API.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int EnvironmentRating { get; set; }

    public int PeopleRating { get; set; }

    public int AnimalRating { get; set; }

    public string RatingDescription { get; set; } = null!;

    public string[] Categories { get; set; } = null!;

    public string[] Ranges { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
