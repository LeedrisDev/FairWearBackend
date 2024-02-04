using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;

namespace BrandAndProduct.Service.Business.IntegrationEventBusiness;

/// <summary>
/// Interface for the IntegrationEventBusiness.
/// </summary>
public interface IIntegrationEventBusiness
{
    /// <summary>Gets all the IntegrationEvents.</summary>
    /// <returns>A list of <see cref="IntegrationEventDto"/>.</returns>
    Task<ProcessingStatusResponse<IEnumerable<IntegrationEventDto>>> GetAllIntegrationEventsAsync();


    /// <summary>Creates a IntegrationEvent.</summary>
    /// <param name="integrationEventDto">The IntegrationEvent to create.</param>
    /// <returns>A <see cref="IntegrationEventDto"/>.</returns>
    Task<ProcessingStatusResponse<IntegrationEventDto>> CreateIntegrationEventAsync(
        IntegrationEventDto integrationEventDto);

    /// <summary>Deletes a IntegrationEvent.</summary>
    /// <param name="id">The Id of the IntegrationEvent to delete.</param>
    /// <returns>A <see cref="Task"/>.</returns>
    Task<ProcessingStatusResponse<IntegrationEventDto>> DeleteIntegrationEventAsync(int id);
}