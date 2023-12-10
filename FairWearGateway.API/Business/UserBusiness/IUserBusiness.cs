using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserBusiness;

/// <summary>
/// Represents business operations for managing users.
/// </summary>
public interface IUserBusiness
{
    /// <summary>
    /// Retrieves a user by Firebase ID.
    /// </summary>
    /// <param name="id">The Firebase ID of the user.</param>
    /// <returns>A processing status response containing the user.</returns>
    ProcessingStatusResponse<User> GetUserByFirebaseId(string id);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user details to create.</param>
    /// <returns>A processing status response containing the created user.</returns>
    ProcessingStatusResponse<User> CreateUser(User request);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="request">The user details to update.</param>
    /// <returns>A processing status response containing the updated user.</returns>
    ProcessingStatusResponse<User> UpdateUser(User request);

    /// <summary>
    /// Deletes a user by Firebase ID.
    /// </summary>
    /// <param name="id">The Firebase ID of the user.</param>
    /// <returns>A processing status response indicating the result of the deletion.</returns>
    ProcessingStatusResponse<Empty> DeleteUserByFirebaseId(string id);
}