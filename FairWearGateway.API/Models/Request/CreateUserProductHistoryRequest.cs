namespace FairWearGateway.API.Models.Request;

/// <summary>
/// Represents a request to create a new user product history.
/// </summary>
public class CreateUserProductHistoryRequest
{
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the product.
    /// </summary>
    public long ProductId { get; set; }
}
