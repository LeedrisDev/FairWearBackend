using System.Net;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Users.Service.Business.UserBusiness;
using Users.Service.Models.Dto;

namespace Users.Service.Services;

/// <summary>
/// Service for user operations.
/// </summary>
public class UserService : Service.UserService.UserServiceBase
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserBusiness _userBusiness;

    /// <summary>
    /// Constructor for UserService.
    /// </summary>
    /// <param name="userBusiness">The user business service.</param>
    /// <param name="logger">The logger for UserService.</param>
    /// <param name="mapper">The mapper for UserService.</param>
    public UserService(IUserBusiness userBusiness, ILogger<UserService> logger, IMapper mapper)
    {
        _userBusiness = userBusiness;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all users based on provided filters.
    /// </summary>
    /// <param name="request">The list of filters for querying users.</param>
    /// <param name="responseStream">The stream to write the users to.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task GetAllUsers(UserFilterList request, IServerStreamWriter<User> responseStream,
        ServerCallContext context)
    {
        var filters = new Dictionary<string, string>();

        foreach (UserFilter filter in request.Filters)
        {
            filters.Add(filter.Key, filter.Value);
        }

        var userList = await _userBusiness.GetAllUsersAsync(filters);

        if (userList.Status != HttpStatusCode.OK)
        {
            _logger.LogError("Error while retrieving users: {ErrorMessage}", userList.ErrorMessage);
            throw new RpcException(new Status(StatusCode.Internal, userList.ErrorMessage));
        }

        foreach (var user in userList.Object)
        {
            await responseStream.WriteAsync(_mapper.Map<User>(user));
        }
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="request">The request containing the user's ID.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The user corresponding to the provided ID.</returns>
    public override async Task<User> GetUserById(UserRequest request, ServerCallContext context)
    {
        var user = await _userBusiness.GetUserByIdAsync(request.Id);
        if (user.Status != HttpStatusCode.OK)
            _logger.LogError("Error while retrieving user: {ErrorMessage}", user.ErrorMessage);

        return user.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound, user.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<User>(user.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, user.ErrorMessage))
        };
    }

    /// <summary>
    /// Retrieves a user by their Firebase ID.
    /// </summary>
    /// <param name="request">The request containing the user's Firebase ID.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The user corresponding to the provided Firebase ID.</returns>
    public override async Task<User> GetUserByFirebaseId(UserFirebaseRequest request, ServerCallContext context)
    {
        var user = await _userBusiness.GetUserByFirebaseIdAsync(request.Id);
        if (user.Status != HttpStatusCode.OK)
            _logger.LogError("Error while retrieving user: {ErrorMessage}", user.ErrorMessage);

        return user.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound, user.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<User>(user.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, user.ErrorMessage))
        };
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user details to create.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The newly created user.</returns>
    public override async Task<User> CreateUser(User request, ServerCallContext context)
    {
        var createdUser = await _userBusiness.CreateUserAsync(_mapper.Map<UserDto>(request));
        if (createdUser.Status != HttpStatusCode.OK)
            _logger.LogError("Error while creating user: {ErrorMessage}", createdUser.ErrorMessage);
        return createdUser.Status switch
        {
            HttpStatusCode.OK => _mapper.Map<User>(createdUser.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, createdUser.ErrorMessage))
        };
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="request">The user details to update.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The updated user.</returns>
    public override async Task<User> UpdateUser(User request, ServerCallContext context)
    {
        var updatedUser = await _userBusiness.UpdateUserAsync(_mapper.Map<UserDto>(request));
        if (updatedUser.Status != HttpStatusCode.OK)
            _logger.LogError("Error while updating users: {ErrorMessage}", updatedUser.ErrorMessage);
        return updatedUser.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                updatedUser.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<User>(updatedUser.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, updatedUser.ErrorMessage))
        };
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="request">The request containing the user's ID.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>An Empty response.</returns>
    public override async Task<Empty> DeleteUser(UserRequest request, ServerCallContext context)
    {
        var deleteUser = await _userBusiness.DeleteUserAsync(request.Id);

        return deleteUser.Status switch
        {
            HttpStatusCode.OK => new Empty(),
            _ => throw new RpcException(new Status(StatusCode.Internal, deleteUser.ErrorMessage))
        };
    }

    /// <summary>
    /// Deletes a user by their Firebase ID.
    /// </summary>
    /// <param name="request">The request containing the user's Firebase ID.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>An Empty response.</returns>
    public override async Task<Empty> DeleteUserByFirebaseId(UserFirebaseRequest request, ServerCallContext context)
    {
        var deleteUser = await _userBusiness.DeleteUserByFirebaseIdAsync(request.Id);

        return deleteUser.Status switch
        {
            HttpStatusCode.OK => new Empty(),
            _ => throw new RpcException(new Status(StatusCode.Internal, deleteUser.ErrorMessage))
        };
    }
}