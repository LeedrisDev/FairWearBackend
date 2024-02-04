using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserProductHistoryBusiness;

/// <summary>
/// Business logic interface for managing user product history.
/// </summary>
public interface IUserProductHistoryBusiness
{
    /// <summary>
    /// Retrieves user product history by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The processing status response containing user product history response.</returns>
    ProcessingStatusResponse<GetUserProductHistoryResponse> GetUserProductHistoryByUserId(long userId);

    /// <summary>
    /// Creates user product history.
    /// </summary>
    /// <param name="request">The request to create user product history.</param>
    /// <returns>The processing status response containing the created user product history.</returns>
    ProcessingStatusResponse<UserProductHistory> CreateUserProductHistory(UserProductHistory request);

    /// <summary>
    /// Updates user product history.
    /// </summary>
    /// <param name="request">The user product history details to update.</param>
    /// <returns>The processing status response containing the updated user product history.</returns>
    ProcessingStatusResponse<UserProductHistory> UpdateUserProductHistory(UserProductHistory request);

    /// <summary>
    /// Deletes user product history by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The processing status response containing the deleted user product history.</returns>
    ProcessingStatusResponse<Empty> DeleteUserProductHistory(long userId);
}