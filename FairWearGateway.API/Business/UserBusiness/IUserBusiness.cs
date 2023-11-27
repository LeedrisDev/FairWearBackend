using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserBusiness;

public interface IUserBusiness
{
    ProcessingStatusResponse<User> GetUserByFirebaseId(string id);
    ProcessingStatusResponse<User> CreateUser(User request);
    ProcessingStatusResponse<User> UpdateUser(User request);
    ProcessingStatusResponse<Empty> DeleteUserByFirebaseId(string id);
}