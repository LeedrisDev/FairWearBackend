using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserExperienceBusiness;

public interface IUserExperienceBusiness
{
    ProcessingStatusResponse<UserExperience> GetUserExperienceByUserId(long userId);
    ProcessingStatusResponse<UserExperience> CreateUserExperience(UserExperience request);
    ProcessingStatusResponse<UserExperience> UpdateUserExperience(UserExperience request);
    ProcessingStatusResponse<Empty> DeleteUserExperience(long userId);
}