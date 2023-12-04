namespace FairWearGateway.API.Models.Request;

/// <summary>
/// Represents a request to create a new user product history.
/// </summary>
public class CreateUserProductHistoryRequest
{
    public long UserId { get; set; }
    public long ProductId { get; set; }
}