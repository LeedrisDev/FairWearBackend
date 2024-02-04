using System.Net;
using Users.Service.DataAccess.Filters;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.UserExperienceBusiness;

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
    public Task<ProcessingStatusResponse<IEnumerable<UserExperienceDto>>> GetAllUserExperiencesAsync(
        Dictionary<string, string> filters)
    {
        var filter = _filterFactory.CreateFilter(filters);
        return _userExperienceRepository.GetAllAsync(filter);
    }

    /// <inheritdoc/>
    public Task<ProcessingStatusResponse<UserExperienceDto>> GetUserExperienceByIdAsync(long id)
    {
        return _userExperienceRepository.GetByIdAsync(id);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<UserExperienceDto>> GetUserExperienceByUserIdAsync(long id)
    {
        var processingStatusResponse = new ProcessingStatusResponse<UserExperienceDto>();

        var dict = new Dictionary<string, string>()
        {
            { "UserId", id.ToString() }
        };

        var filter = _filterFactory.CreateFilter(dict);

        var results = await _userExperienceRepository.GetAllAsync(filter);

        if (!results.Object.Any())
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"UserExperience with userId {id} not found.";
        }
        else
        {
            processingStatusResponse.Object = results.Object.First();
        }

        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public Task<ProcessingStatusResponse<UserExperienceDto>> CreateUserExperienceAsync(
        UserExperienceDto userExperienceDto)
    {
        return _userExperienceRepository.AddAsync(userExperienceDto);
    }

    /// <inheritdoc/>
    public Task<ProcessingStatusResponse<UserExperienceDto>> UpdateUserExperienceAsync(
        UserExperienceDto userExperienceDto)
    {
        return _userExperienceRepository.UpdateAsync(userExperienceDto);
    }

    /// <inheritdoc/>
    public Task<ProcessingStatusResponse<UserExperienceDto>> DeleteUserExperienceAsync(long id)
    {
        return _userExperienceRepository.DeleteAsync(id);
    }
}