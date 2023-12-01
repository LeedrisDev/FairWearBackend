using FairWearGateway.API.DataAccess.UserProductHistoryData;
using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserProductHistoryBusiness;

public class UserProductHistoryBusiness : IUserProductHistoryBusiness
{
    
    private readonly IUserProductHistoryData _userProductHistoryData;

    /// <summary>Initializes a new instance of the <see cref="UserProductHistoryBusiness"/> class.</summary>
    public UserProductHistoryBusiness(IUserProductHistoryData userProductHistoryData)
    {
        _userProductHistoryData = userProductHistoryData;
    }

    public ProcessingStatusResponse<GetUserProductHistoryResponse> GetUserProductHistoryByUserId(long userId)
    {
        return _userProductHistoryData.GetUserProductHistoryByUserId(userId);
    }

    public ProcessingStatusResponse<UserProductHistory> CreateUserProductHistory(UserProductHistory request)
    {
        return _userProductHistoryData.CreateUserProductHistory(request);
    }

    public ProcessingStatusResponse<UserProductHistory> UpdateUserProductHistory(UserProductHistory request)
    {
        return _userProductHistoryData.UpdateUserProductHistory(request);
    }

    public ProcessingStatusResponse<Empty> DeleteUserProductHistory(long userId)
    {
        return _userProductHistoryData.DeleteUserProductHistory(userId);
    }
    
}