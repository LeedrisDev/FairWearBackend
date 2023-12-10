using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.DataAccess.UserData;

/// <summary>
/// Interface for user data operations.
/// </summary>
public interface IUserData
{
    /// <summary>
    /// Gets a user by their Firebase ID.
    /// </summary>
    /// <param name="id">The Firebase ID of the user.</param>
    /// <returns>A processing status response containing the user or an error response.</returns>
    ProcessingStatusResponse<User> GetUserByFirebaseId(string id);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user details to create.</param>
    /// <returns>A processing status response containing the created user or an error response.</returns>
    ProcessingStatusResponse<User> CreateUser(User request);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="request">The user details to update.</param>
    /// <returns>A processing status response containing the updated user or an error response.</returns>
    ProcessingStatusResponse<User> UpdateUser(User request);

    /// <summary>
    /// Deletes a user by their Firebase ID.
    /// </summary>
    /// <param name="id">The Firebase ID of the user to delete.</param>
    /// <returns>A processing status response indicating the result of the deletion or an error response.</returns>
    ProcessingStatusResponse<Empty> DeleteUserByFirebaseId(string id);
}
