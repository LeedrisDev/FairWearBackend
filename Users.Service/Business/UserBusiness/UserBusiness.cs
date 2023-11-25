using Users.Service.DataAccess.Filters;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.UserBusiness
{
    /// <summary>Business logic for users database actions </summary>
    public class UserBusiness : IUserBusiness
    {
        private readonly IFilterFactory<IFilter> _filterFactory;

        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor for UserBusiness.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="filterFactory"></param>
        public UserBusiness(IUserRepository userRepository, IFilterFactory<IFilter> filterFactory)
        {
            _userRepository = userRepository;
            _filterFactory = filterFactory;
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<IEnumerable<UserDto>>> GetAllUsersAsync(
            Dictionary<string, string> filters)
        {
            var filter = _filterFactory.CreateFilter(filters);

            return await _userRepository.GetAllAsync(filter);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserDto>> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserDto>> CreateUserAsync(UserDto userDto)
        {
            return await _userRepository.AddAsync(userDto);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserDto>> UpdateUserAsync(UserDto userDto)
        {
            return await _userRepository.UpdateAsync(userDto);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserDto>> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }
}