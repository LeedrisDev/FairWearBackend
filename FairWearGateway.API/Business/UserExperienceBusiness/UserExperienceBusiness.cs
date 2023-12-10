using FairWearGateway.API.DataAccess.UserExperienceData;
using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserExperienceBusiness;

/// <summary>
/// Represents business operations for managing user experiences.
/// </summary>
public class UserExperienceBusiness : IUserExperienceBusiness
{
    private readonly IUserExperienceData _userExperienceData;

    /// <summary>Initializes a new instance of the <see cref="UserExperienceBusiness"/> class.</summary>
    public UserExperienceBusiness(IUserExperienceData userExperienceData)
    {
        _userExperienceData = userExperienceData;
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<UserExperience> GetUserExperienceByUserId(long userId)
    {
        return _userExperienceData.GetUserExperienceByUserId(userId);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<UserExperience> CreateUserExperience(UserExperience request)
    {
        return _userExperienceData.CreateUserExperience(request);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<UserExperience> UpdateUserExperience(UserExperience request)
    {
        return _userExperienceData.UpdateUserExperience(request);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<Empty> DeleteUserExperience(long userId)
    {
        return _userExperienceData.DeleteUserExperience(userId);
    }
}