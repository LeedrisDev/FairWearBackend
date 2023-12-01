using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserProductHistoryBusiness;

public interface IUserProductHistoryBusiness
{
    ProcessingStatusResponse<GetUserProductHistoryResponse> GetUserProductHistoryByUserId(long userId);
    ProcessingStatusResponse<UserProductHistory> CreateUserProductHistory(UserProductHistory request);
    ProcessingStatusResponse<UserProductHistory> UpdateUserProductHistory(UserProductHistory request);
    ProcessingStatusResponse<Empty> DeleteUserProductHistory(long userId);
}