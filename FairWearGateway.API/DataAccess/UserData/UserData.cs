using System.Net;
using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Users.Service;

namespace FairWearGateway.API.DataAccess.UserData;

/// <summary>
/// Class that call the appropriate microservice to get all is related to users.
/// </summary>
public class UserData : IUserData
{
    private readonly UserService.UserServiceClient _client;

    /// <summary>Constructor</summary>
    public UserData(GrpcClientFactory grpcClientFactory)
    {
        _client = grpcClientFactory.CreateClient<UserService.UserServiceClient>("UserService");
    }

    /// <inheritdoc />
    public ProcessingStatusResponse<User> GetUserByFirebaseId(string id)
    {
        var processingStatusResponse = new ProcessingStatusResponse<User>();

        var data = new UserFirebaseRequest() { Id = id };

        try
        {
            var response = _client.GetUserByFirebaseId(data);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = $"User with firebase id {id} could not be found";
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
    public ProcessingStatusResponse<User> CreateUser(User request)
    {
        var processingStatusResponse = new ProcessingStatusResponse<User>();
        try
        {
            var response = _client.CreateUser(request);
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
    public ProcessingStatusResponse<User> UpdateUser(User request)
    {
        var processingStatusResponse = new ProcessingStatusResponse<User>();
        try
        {
            var response = _client.UpdateUser(request);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = "User could not be found";
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
    public ProcessingStatusResponse<Empty> DeleteUserByFirebaseId(string id)
    {
        var processingStatusResponse = new ProcessingStatusResponse<Empty>();
        var request = new UserFirebaseRequest() { Id = id };

        try
        {
            var response = _client.DeleteUserByFirebaseId(request);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = $"User with firebase id {id} could not be found";
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