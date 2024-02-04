using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.DataAccess.UserProductHistoryData;

/// <summary>
/// Interface for user product history data operations.
/// </summary>
public interface IUserProductHistoryData
{
    /// <summary>
    /// Gets user product history by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A processing status response containing the user product history or an error response.</returns>
    ProcessingStatusResponse<GetUserProductHistoryResponse> GetUserProductHistoryByUserId(long userId);

    /// <summary>
    /// Creates a new user product history.
    /// </summary>
    /// <param name="request">The user product history details to create.</param>
    /// <returns>A processing status response containing the created user product history or an error response.</returns>
    ProcessingStatusResponse<UserProductHistory> CreateUserProductHistory(UserProductHistory request);

    /// <summary>
    /// Updates an existing user product history.
    /// </summary>
    /// <param name="request">The user product history details to update.</param>
    /// <returns>A processing status response containing the updated user product history or an error response.</returns>
    ProcessingStatusResponse<UserProductHistory> UpdateUserProductHistory(UserProductHistory request);

    /// <summary>
    /// Deletes user product history by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A processing status response indicating the result of the deletion or an error response.</returns>
    ProcessingStatusResponse<Empty> DeleteUserProductHistory(long userId);
}
