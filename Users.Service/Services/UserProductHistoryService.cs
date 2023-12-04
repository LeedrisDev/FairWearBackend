using System.Net;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Users.Service.Business.UserProductHistoryBusiness;
using Users.Service.Models.Dto;

namespace Users.Service.Services;

/// <summary>
/// Service for user product history operations.
/// </summary>
public class UserProductHistoryService : Service.UserProductHistoryService.UserProductHistoryServiceBase
{
    private readonly ILogger<UserProductHistoryService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserProductHistoryBusiness _userProductHistoryBusiness;

    /// <summary>
    /// Constructor for UserProductHistoryService.
    /// </summary>
    /// <param name="userProductHistoryProductHistoryBusiness">The user product history business service.</param>
    /// <param name="logger">The logger for UserProductHistoryService.</param>
    /// <param name="mapper">The mapper for UserProductHistoryService.</param>
    public UserProductHistoryService(IUserProductHistoryBusiness userProductHistoryProductHistoryBusiness,
        ILogger<UserProductHistoryService> logger, IMapper mapper)
    {
        _userProductHistoryBusiness = userProductHistoryProductHistoryBusiness;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all history for a user.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="RpcException"></exception>
    public override async Task<GetUserProductHistoryResponse> GetUserProductHistory(UserProductHistoryRequest request,
        ServerCallContext context)
    {
        var history = await _userProductHistoryBusiness.GetUserProductHistoryComplete(request.Id);
        if (history.Status != HttpStatusCode.OK)
            _logger.LogError("Error while retrieving user history: {ErrorMessage}", history.ErrorMessage);

        return history.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound, history.ErrorMessage)),
            HttpStatusCode.OK => history.Object,
            _ => throw new RpcException(new Status(StatusCode.Internal, history.ErrorMessage))
        };
    }

    /// <summary>
    /// Creates a new userProductHistory.
    /// </summary>
    /// <param name="request">The user details to create.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The newly created user.</returns>
    public override async Task<UserProductHistory> AddUserProductHistory(UserProductHistory request,
        ServerCallContext context)
    {
        var createdHistory =
            await _userProductHistoryBusiness.CreateUserProductHistoryAsync(
                _mapper.Map<UserProductHistoryDto>(request));
        if (createdHistory.Status != HttpStatusCode.OK)
            _logger.LogError("Error while creating user history: {ErrorMessage}", createdHistory.ErrorMessage);

        var response = new UserProductHistory()
        {
            Id = createdHistory.Object.Id,
            ProductId = createdHistory.Object.ProductId,
            UserId = createdHistory.Object.UserId,
            Timestamp = Timestamp.FromDateTime(createdHistory.Object.Timestamp ?? DateTime.UtcNow.ToUniversalTime())
        };
        return createdHistory.Status switch
        {
            HttpStatusCode.OK => response,
            _ => throw new RpcException(new Status(StatusCode.Internal, createdHistory.ErrorMessage))
        };
    }

    /// <summary>
    /// Updates an existing userProductHistory.
    /// </summary>
    /// <param name="request">The userProductHistory details to update.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The updated userProductHistory.</returns>
    public override async Task<UserProductHistory> UpdateUserProductHistory(UserProductHistory request,
        ServerCallContext context)
    {
        var updatedUserProductHistory =
            await _userProductHistoryBusiness.UpdateUserProductHistoryAsync(
                _mapper.Map<UserProductHistoryDto>(request));
        if (updatedUserProductHistory.Status != HttpStatusCode.OK)
            _logger.LogError("Error while updating user history: {ErrorMessage}",
                updatedUserProductHistory.ErrorMessage);
        return updatedUserProductHistory.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                updatedUserProductHistory.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<UserProductHistory>(updatedUserProductHistory.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, updatedUserProductHistory.ErrorMessage))
        };
    }

    /// <summary>
    /// Deletes a userProductHistory by their ID.
    /// </summary>
    /// <param name="request">The request containing the userProductHistory's ID.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>An Empty response.</returns>
    public override async Task<Empty> DeleteUserProductHistory(UserProductHistoryRequest request,
        ServerCallContext context)
    {
        var deleteUserProductHistory = await _userProductHistoryBusiness.DeleteUserProductHistoryAsync(request.Id);

        return deleteUserProductHistory.Status switch
        {
            HttpStatusCode.OK => new Empty(),
            _ => throw new RpcException(new Status(StatusCode.Internal, deleteUserProductHistory.ErrorMessage))
        };
    }
}