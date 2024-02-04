namespace BrandAndProduct.Service.Models.Dto;

/// <summary>
/// Dto for IntegrationEvent.
/// </summary>
public class IntegrationEventDto : IObjectWithId
{
    /// <summary>The Type of the Event.</summary>
    public string Event { get; set; } = null!;

    /// <summary>The Content of the Event.</summary>
    public string Data { get; set; } = null!;

    /// <summary>The Id of the Event.</summary>
    public int Id { get; set; }
}