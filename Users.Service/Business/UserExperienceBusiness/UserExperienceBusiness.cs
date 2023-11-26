using Users.Service.DataAccess.Filters;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.UserExperienceBusiness
{
    /// <summary>Business logic for userExperiences database actions </summary>
    public class UserExperienceBusiness : IUserExperienceBusiness
    {
        private readonly IFilterFactory<IFilter> _filterFactory;
        private readonly IUserExperienceRepository _userExperienceRepository;

        /// <summary>
        /// Constructor for UserExperienceBusiness.
        /// </summary>
        /// <param name="userExperienceRepository"></param>
        /// <param name="filterFactory"></param>
        public UserExperienceBusiness(IUserExperienceRepository userExperienceRepository,
            IFilterFactory<IFilter> filterFactory)
        {
            _userExperienceRepository = userExperienceRepository;
            _filterFactory = filterFactory;
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<IEnumerable<UserExperienceDto>>> GetAllUserExperiencesAsync(
            Dictionary<string, string> filters)
        {
            var filter = _filterFactory.CreateFilter(filters);

            return await _userExperienceRepository.GetAllAsync(filter);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserExperienceDto>> GetUserExperienceByIdAsync(long id)
        {
            return await _userExperienceRepository.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserExperienceDto>> CreateUserExperienceAsync(
            UserExperienceDto userExperienceDto)
        {
            return await _userExperienceRepository.AddAsync(userExperienceDto);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserExperienceDto>> UpdateUserExperienceAsync(
            UserExperienceDto userExperienceDto)
        {
            return await _userExperienceRepository.UpdateAsync(userExperienceDto);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<UserExperienceDto>> DeleteUserExperienceAsync(long id)
        {
            return await _userExperienceRepository.DeleteAsync(id);
        }
    }
}