namespace Users.Service.Models.Entity;

/// <summary>
/// Represents a Product entity.
/// </summary>
public class ProductEntity : IObjectWithId
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the rating of the product.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Gets or sets the collection of user product histories associated with this product.
    /// </summary>
    public virtual ICollection<UserProductHistoryEntity> UserProductHistories { get; set; } =
        new List<UserProductHistoryEntity>();

    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public long Id { get; set; }
}