using Users.Service.DataAccess.Filters;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.UserProductHistoryBusiness
{
    /// <summary>Business logic for userProductHistorys database actions </summary>
    public class UserProductHistoryBusiness : IUserProductHistoryBusiness
    {
        private readonly IFilterFactory<IFilter> _filterFactory;
        private readonly IUserProductHistoryRepository _userProductHistoryRepository;

        /// <summary>
        /// Constructor for UserProductHistoryBusiness.
        /// </summary>
        /// <param name="userProductHistoryRepository"></param>
        /// <param name="filterFactory"></param>
        public UserProductHistoryBusiness(IUserProductHistoryRepository userProductHistoryRepository,
            IFilterFactory<IFilter> filterFactory)
        {
            _userProductHistoryRepository = userProductHistoryRepository;
            _filterFactory = filterFactory;
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<IEnumerable<UserProductHistoryDto>>> GetAllUserProductHistorysAsync(
            Dictionary<string, string> filters)
        {
            var filter = _filterFactory.CreateFilter(filters);

            return await _userProductHistoryRepository.GetAllAsync(filter);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserProductHistoryDto>> GetUserProductHistoryByIdAsync(int id)
        {
            return await _userProductHistoryRepository.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserProductHistoryDto>> CreateUserProductHistoryAsync(
            UserProductHistoryDto userProductHistoryDto)
        {
            return await _userProductHistoryRepository.AddAsync(userProductHistoryDto);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserProductHistoryDto>> UpdateUserProductHistoryAsync(
            UserProductHistoryDto userProductHistoryDto)
        {
            return await _userProductHistoryRepository.UpdateAsync(userProductHistoryDto);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserProductHistoryDto>> DeleteUserProductHistoryAsync(int id)
        {
            return await _userProductHistoryRepository.DeleteAsync(id);
        }
    }
}