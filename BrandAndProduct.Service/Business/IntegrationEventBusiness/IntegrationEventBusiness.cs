using BrandAndProduct.Service.DataAccess.Filters;
using BrandAndProduct.Service.DataAccess.IRepositories;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;

namespace BrandAndProduct.Service.Business.IntegrationEventBusiness;

/// <summary>
/// Class representing a business for the IntegrationEvent.
/// </summary>
public class IntegrationEventBusiness : IIntegrationEventBusiness
{
    private readonly IFilterFactory<IFilter> _filterFactory;
    private readonly IIntegrationEventRepository _integrationEventRepository;

    /// <summary>
    /// Constructor for IntegrationEventBusiness.
    /// </summary>
    /// <param name="integrationEventRepository"></param>
    /// <param name="filterFactory"></param>
    public IntegrationEventBusiness(IIntegrationEventRepository integrationEventRepository,
        IFilterFactory<IFilter> filterFactory)
    {
        _integrationEventRepository = integrationEventRepository;
        _filterFactory = filterFactory;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<IntegrationEventDto>>> GetAllIntegrationEventsAsync()
    {
        var filter = _filterFactory.CreateFilter(new Dictionary<string, string>());

        return await _integrationEventRepository.GetAllAsync(filter);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IntegrationEventDto>> CreateIntegrationEventAsync(
        IntegrationEventDto integrationEventDto)
    {
        return await _integrationEventRepository.AddAsync(integrationEventDto);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IntegrationEventDto>> DeleteIntegrationEventAsync(int id)
    {
        return await _integrationEventRepository.DeleteAsync(id);
    }
}