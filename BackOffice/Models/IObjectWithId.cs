namespace BackOffice.Models;

/// <summary>Represents an object with a unique identifier.</summary>
public interface IObjectWithId
{
    /// <summary>Gets or sets the unique identifier of the object.</summary>
    long Id { get; set; }
}
