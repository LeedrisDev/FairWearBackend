namespace Users.Service.Models;

/// <summary>Interface for objects with an Id</summary>
public interface IObjectWithId
{
    /// <summary>Id of the object</summary>
    public long Id { get; set; }
}