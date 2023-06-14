namespace BrandAndProductDatabase.API.Models.Request;

/// <summary>Class to handle the request about brands.</summary>
public class BrandRequest
{
    /// <summary>Name of the brand.</summary>
    public string Name { get; set; } = string.Empty;
}