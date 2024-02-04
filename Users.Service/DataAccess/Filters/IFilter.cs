namespace Users.Service.DataAccess.Filters;

/// <summary>
/// Represents a filter used in querying data with a specific property name.
/// </summary>
public interface IFilter
{
    /// <summary>
    /// Gets the name of the property associated with the filter.
    /// </summary>
    string PropertyName { get; }
}
