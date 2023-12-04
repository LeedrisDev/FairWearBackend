using FairWearGateway.API.DataAccess.UserExperienceData;
using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserExperienceBusiness;

public class UserExperienceBusiness : IUserExperienceBusiness
{
    private readonly IUserExperienceData _userExperienceData;

    /// <summary>Initializes a new instance of the <see cref="UserExperienceBusiness"/> class.</summary>
    public UserExperienceBusiness(IUserExperienceData userExperienceData)
    {
        _userExperienceData = userExperienceData;
    }

    public ProcessingStatusResponse<UserExperience> GetUserExperienceByUserId(long userId)
    {
        return _userExperienceData.GetUserExperienceByUserId(userId);
    }

    public ProcessingStatusResponse<UserExperience> CreateUserExperience(UserExperience request)
    {
        return _userExperienceData.CreateUserExperience(request);
    }

    public ProcessingStatusResponse<UserExperience> UpdateUserExperience(UserExperience request)
    {
        return _userExperienceData.UpdateUserExperience(request);
    }

    public ProcessingStatusResponse<Empty> DeleteUserExperience(long userId)
    {
        return _userExperienceData.DeleteUserExperience(userId);
    }
}