namespace BrandAndProduct.Service.Services;

/// <summary>
///  Interface for Integration event sender service
/// </summary>
public interface IIntegrationEventSenderService
{
    /// <summary>
    ///  Publish outstanding integration events
    /// </summary>
    void StartPublishingOutstandingIntegrationEvents();
}