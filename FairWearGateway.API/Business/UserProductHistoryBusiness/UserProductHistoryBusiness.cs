using FairWearGateway.API.DataAccess.UserProductHistoryData;
using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserProductHistoryBusiness;

/// <summary>
/// Represents business operations for managing user product history.
/// </summary>
public class UserProductHistoryBusiness : IUserProductHistoryBusiness
{
    
    private readonly IUserProductHistoryData _userProductHistoryData;

    /// <summary>Initializes a new instance of the <see cref="UserProductHistoryBusiness"/> class.</summary>
    public UserProductHistoryBusiness(IUserProductHistoryData userProductHistoryData)
    {
        _userProductHistoryData = userProductHistoryData;
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<GetUserProductHistoryResponse> GetUserProductHistoryByUserId(long userId)
    {
        return _userProductHistoryData.GetUserProductHistoryByUserId(userId);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<UserProductHistory> CreateUserProductHistory(UserProductHistory request)
    {
        return _userProductHistoryData.CreateUserProductHistory(request);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<UserProductHistory> UpdateUserProductHistory(UserProductHistory request)
    {
        return _userProductHistoryData.UpdateUserProductHistory(request);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<Empty> DeleteUserProductHistory(long userId)
    {
        return _userProductHistoryData.DeleteUserProductHistory(userId);
    }
    
}