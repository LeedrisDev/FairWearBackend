namespace BrandAndProduct.Service.DataAccess.Filters;

/// <summary>
/// Interface for Filter
/// </summary>
public interface IFilter
{
    /// <summary>
    /// The value to compare against
    /// </summary>
    string PropertyName { get; }
}