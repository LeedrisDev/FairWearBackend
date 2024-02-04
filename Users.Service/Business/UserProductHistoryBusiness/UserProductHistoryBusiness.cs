using AutoMapper;
using Users.Service.DataAccess.Filters;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.UserProductHistoryBusiness;

/// <summary>Business logic for userProductHistorys database actions </summary>
public class UserProductHistoryBusiness : IUserProductHistoryBusiness
{
    private readonly IFilterFactory<IFilter> _filterFactory;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IUserProductHistoryRepository _userProductHistoryRepository;

    /// <summary>
    /// Constructor for UserProductHistoryBusiness.
    /// </summary>
    /// <param name="userProductHistoryRepository"></param>
    /// <param name="productRepository"></param>
    /// <param name="filterFactory"></param>
    /// <param name="mapper"></param>
    public UserProductHistoryBusiness(IUserProductHistoryRepository userProductHistoryRepository,
        IProductRepository productRepository,
        IFilterFactory<IFilter> filterFactory, IMapper mapper)
    {
        _userProductHistoryRepository = userProductHistoryRepository;
        _productRepository = productRepository;
        _filterFactory = filterFactory;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<GetUserProductHistoryResponse>>
        GetUserProductHistoryComplete(long id)
    {
        var processingStatusResponse = new ProcessingStatusResponse<GetUserProductHistoryResponse>();

        var dict = new Dictionary<string, string>()
        {
            { "UserId", id.ToString() }
        };

        var filter = _filterFactory.CreateFilter(dict);

        var userProductHistory = await _userProductHistoryRepository.GetAllAsync(filter);

        var product = new List<Product>();

        foreach (var userProductHistoryDto in userProductHistory.Object)
        {
            var productDto = await _productRepository.GetByIdAsync(userProductHistoryDto.ProductId);
            product.Add(_mapper.Map<Product>(productDto.Object));
        }

        processingStatusResponse.Object = new GetUserProductHistoryResponse()
        {
            UserId = id,
        };

        processingStatusResponse.Object.Products.AddRange(product);

        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public Task<ProcessingStatusResponse<IEnumerable<UserProductHistoryDto>>> GetAllUserProductHistorysAsync(
        Dictionary<string, string> filters)
    {
        var filter = _filterFactory.CreateFilter(filters);
        return _userProductHistoryRepository.GetAllAsync(filter);
    }

    /// <inheritdoc/>
    public Task<ProcessingStatusResponse<UserProductHistoryDto>> GetUserProductHistoryByIdAsync(long id)
    {
        return _userProductHistoryRepository.GetByIdAsync(id);
    }

    /// <inheritdoc/>
    public Task<ProcessingStatusResponse<UserProductHistoryDto>> CreateUserProductHistoryAsync(
        UserProductHistoryDto userProductHistoryDto)
    {
        return _userProductHistoryRepository.AddAsync(userProductHistoryDto);
    }

    /// <inheritdoc/>
    public Task<ProcessingStatusResponse<UserProductHistoryDto>> UpdateUserProductHistoryAsync(
        UserProductHistoryDto userProductHistoryDto)
    {
        return _userProductHistoryRepository.UpdateAsync(userProductHistoryDto);
    }

    /// <inheritdoc/>
    public Task<ProcessingStatusResponse<UserProductHistoryDto>> DeleteUserProductHistoryAsync(long id)
    {
        return _userProductHistoryRepository.DeleteAsync(id);
    }
}