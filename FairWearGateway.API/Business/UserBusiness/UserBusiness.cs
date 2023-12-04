using FairWearGateway.API.DataAccess.UserData;
using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserBusiness;

public class UserBusiness : IUserBusiness
{
    private readonly IUserData _userData;

    /// <summary>Initializes a new instance of the <see cref="UserBusiness"/> class.</summary>
    public UserBusiness(IUserData userData)
    {
        _userData = userData;
    }


    public ProcessingStatusResponse<User> GetUserByFirebaseId(string id)
    {
        return _userData.GetUserByFirebaseId(id);
    }

    public ProcessingStatusResponse<User> CreateUser(User request)
    {
        return _userData.CreateUser(request);
    }

    public ProcessingStatusResponse<User> UpdateUser(User request)
    {
        return _userData.UpdateUser(request);
    }

    public ProcessingStatusResponse<Empty> DeleteUserByFirebaseId(string id)
    {
        return _userData.DeleteUserByFirebaseId(id);
    }
}