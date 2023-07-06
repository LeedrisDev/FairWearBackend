namespace BrandAndProductDatabase.Service.Models;

/// <summary>Interface for objects with an Id</summary>
public interface IObjectWithId
{
    /// <summary>Id of the object</summary>
    public int Id { get; set; }
}