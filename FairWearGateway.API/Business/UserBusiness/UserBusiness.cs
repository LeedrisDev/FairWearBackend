using FairWearGateway.API.DataAccess.UserData;
using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Users.Service;

namespace FairWearGateway.API.Business.UserBusiness;

/// <summary>
/// Represents business operations for managing users.
/// </summary>
public class UserBusiness : IUserBusiness
{
    private readonly IUserData _userData;

    /// <summary>Initializes a new instance of the <see cref="UserBusiness"/> class.</summary>
    public UserBusiness(IUserData userData)
    {
        _userData = userData;
    }
    
    /// <inheritdoc/>
    public ProcessingStatusResponse<User> GetUserByFirebaseId(string id)
    {
        return _userData.GetUserByFirebaseId(id);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<User> CreateUser(User request)
    {
        return _userData.CreateUser(request);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<User> UpdateUser(User request)
    {
        return _userData.UpdateUser(request);
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<Empty> DeleteUserByFirebaseId(string id)
    {
        return _userData.DeleteUserByFirebaseId(id);
    }
}