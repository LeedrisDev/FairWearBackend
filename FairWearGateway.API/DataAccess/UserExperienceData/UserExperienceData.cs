using System.Net;
using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Users.Service;

namespace FairWearGateway.API.DataAccess.UserExperienceData;

/// <summary>
/// Data access class for user experience data operations.
/// </summary>
public class UserExperienceData : IUserExperienceData
{
    private readonly UserExperienceService.UserExperienceServiceClient _client;

    /// <summary>Constructor</summary>
    public UserExperienceData(GrpcClientFactory grpcClientFactory)
    {
        _client =
            grpcClientFactory.CreateClient<UserExperienceService.UserExperienceServiceClient>("UserExperienceService");
    }

    /// <inheritdoc />
    public ProcessingStatusResponse<UserExperience> GetUserExperienceByUserId(long userId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<UserExperience>();

        var data = new UserExperienceRequest() { Id = userId };

        try
        {
            var response = _client.GetUserExperienceByUserId(data);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = $"User experience with user id {userId} could not be found";
            }
            else
            {
                processingStatusResponse.Status = HttpStatusCode.InternalServerError;
                processingStatusResponse.ErrorMessage = e.Status.Detail;
            }
        }

        return processingStatusResponse;
    }

    /// <inheritdoc />
    public ProcessingStatusResponse<UserExperience> CreateUserExperience(UserExperience request)
    {
        var processingStatusResponse = new ProcessingStatusResponse<UserExperience>();

        try
        {
            var response = _client.CreateUserExperience(request);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            processingStatusResponse.Status = HttpStatusCode.InternalServerError;
            processingStatusResponse.ErrorMessage = e.Status.Detail;
        }

        return processingStatusResponse;
    }

    /// <inheritdoc />
    public ProcessingStatusResponse<UserExperience> UpdateUserExperience(UserExperience request)
    {
        var processingStatusResponse = new ProcessingStatusResponse<UserExperience>();
        try
        {
            var response = _client.UpdateUserExperience(request);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = "User experience could not be found";
            }
            else
            {
                processingStatusResponse.Status = HttpStatusCode.InternalServerError;
                processingStatusResponse.ErrorMessage = e.Status.Detail;
            }
        }

        return processingStatusResponse;
    }

    /// <inheritdoc />
    public ProcessingStatusResponse<Empty> DeleteUserExperience(long userId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<Empty>();

        var data = new UserExperienceRequest() { Id = userId };

        try
        {
            var response = _client.DeleteUserExperience(data);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = $"User experience with user id {userId} could not be found";
            }
            else
            {
                processingStatusResponse.Status = HttpStatusCode.InternalServerError;
                processingStatusResponse.ErrorMessage = e.Status.Detail;
            }
        }

        return processingStatusResponse;
    }
}