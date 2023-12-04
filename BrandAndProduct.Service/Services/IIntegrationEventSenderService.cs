namespace BrandAndProduct.Service.Services;

public interface IIntegrationEventSenderService
{
    void StartPublishingOutstandingIntegrationEvents();
}