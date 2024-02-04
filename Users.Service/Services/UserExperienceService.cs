using System.Net;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Users.Service.Business.UserExperienceBusiness;
using Users.Service.Models.Dto;

namespace Users.Service.Services;

/// <summary>
/// gRPC service implementation for handling user experience-related operations.
/// </summary>
public class UserExperienceService : Service.UserExperienceService.UserExperienceServiceBase
{
    private readonly ILogger<UserExperienceService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserExperienceBusiness _userExperienceBusiness;

    /// <summary>
    /// Constructor for UserExperienceService.
    /// </summary>
    /// <param name="userExperienceBusiness">The userExperience business service.</param>
    /// <param name="logger">The logger for UserExperienceService.</param>
    /// <param name="mapper">The mapper for UserExperienceService.</param>
    public UserExperienceService(IUserExperienceBusiness userExperienceBusiness, ILogger<UserExperienceService> logger,
        IMapper mapper)
    {
        _userExperienceBusiness = userExperienceBusiness;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all userExperiences based on provided filters.
    /// </summary>
    /// <param name="request">The list of filters for querying userExperiences.</param>
    /// <param name="responseStream">The stream to write the userExperiences to.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task GetAllUserExperiences(UserExperienceFilterList request,
        IServerStreamWriter<UserExperience> responseStream,
        ServerCallContext context)
    {
        var filters = new Dictionary<string, string>();

        foreach (UserExperienceFilter filter in request.Filters)
        {
            filters.Add(filter.Key, filter.Value);
        }

        var userExperienceList = await _userExperienceBusiness.GetAllUserExperiencesAsync(filters);

        if (userExperienceList.Status != HttpStatusCode.OK)
        {
            _logger.LogError("Error while retrieving userExperiences: {ErrorMessage}", userExperienceList.ErrorMessage);
            throw new RpcException(new Status(StatusCode.Internal, userExperienceList.ErrorMessage));
        }

        foreach (var userExperience in userExperienceList.Object)
        {
            await responseStream.WriteAsync(_mapper.Map<UserExperience>(userExperience));
        }
    }

    /// <summary>
    /// Retrieves a userExperience by their ID.
    /// </summary>
    /// <param name="request">The request containing the userExperience's ID.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The userExperience corresponding to the provided ID.</returns>
    public override async Task<UserExperience> GetUserExperienceByUserId(UserExperienceRequest request,
        ServerCallContext context)
    {
        var userExperience = await _userExperienceBusiness.GetUserExperienceByUserIdAsync(request.Id);
        if (userExperience.Status != HttpStatusCode.OK)
            _logger.LogError("Error while retrieving userExperience: {ErrorMessage}", userExperience.ErrorMessage);

        return userExperience.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                userExperience.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<UserExperience>(userExperience.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, userExperience.ErrorMessage))
        };
    }

    /// <summary>
    /// Creates a new userExperience.
    /// </summary>
    /// <param name="request">The userExperience details to create.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The newly created userExperience.</returns>
    public override async Task<UserExperience> CreateUserExperience(UserExperience request, ServerCallContext context)
    {
        var createdUserExperience =
            await _userExperienceBusiness.CreateUserExperienceAsync(_mapper.Map<UserExperienceDto>(request));
        if (createdUserExperience.Status != HttpStatusCode.OK)
            _logger.LogError("Error while creating userExperience: {ErrorMessage}", createdUserExperience.ErrorMessage);
        return createdUserExperience.Status switch
        {
            HttpStatusCode.OK => _mapper.Map<UserExperience>(createdUserExperience.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, createdUserExperience.ErrorMessage))
        };
    }

    /// <summary>
    /// Updates an existing userExperience.
    /// </summary>
    /// <param name="request">The userExperience details to update.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The updated userExperience.</returns>
    public override async Task<UserExperience> UpdateUserExperience(UserExperience request, ServerCallContext context)
    {
        var updatedUserExperience =
            await _userExperienceBusiness.UpdateUserExperienceAsync(_mapper.Map<UserExperienceDto>(request));
        if (updatedUserExperience.Status != HttpStatusCode.OK)
            _logger.LogError("Error while updating userExperiences: {ErrorMessage}",
                updatedUserExperience.ErrorMessage);
        return updatedUserExperience.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                updatedUserExperience.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<UserExperience>(updatedUserExperience.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, updatedUserExperience.ErrorMessage))
        };
    }

    /// <summary>
    /// Deletes a userExperience by their ID.
    /// </summary>
    /// <param name="request">The request containing the userExperience's ID.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>An Empty response.</returns>
    public override async Task<Empty> DeleteUserExperience(UserExperienceRequest request, ServerCallContext context)
    {
        var deleteUserExperience = await _userExperienceBusiness.DeleteUserExperienceAsync(request.Id);

        return deleteUserExperience.Status switch
        {
            HttpStatusCode.OK => new Empty(),
            _ => throw new RpcException(new Status(StatusCode.Internal, deleteUserExperience.ErrorMessage))
        };
    }
}