using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserExperienceBusiness;

/// <summary>
/// Represents business operations for managing user experiences.
/// </summary>
public interface IUserExperienceBusiness
{
    /// <summary>
    /// Retrieves user experience by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A processing status response containing the user experience.</returns>
    ProcessingStatusResponse<UserExperience> GetUserExperienceByUserId(long userId);

    /// <summary>
    /// Creates a new user experience.
    /// </summary>
    /// <param name="request">The user experience details to create.</param>
    /// <returns>A processing status response containing the created user experience.</returns>
    ProcessingStatusResponse<UserExperience> CreateUserExperience(UserExperience request);

    /// <summary>
    /// Updates an existing user experience.
    /// </summary>
    /// <param name="request">The user experience details to update.</param>
    /// <returns>A processing status response containing the updated user experience.</returns>
    ProcessingStatusResponse<UserExperience> UpdateUserExperience(UserExperience request);

    /// <summary>
    /// Deletes user experience by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A processing status response indicating the result of the deletion.</returns>
    ProcessingStatusResponse<Empty> DeleteUserExperience(long userId);
}
