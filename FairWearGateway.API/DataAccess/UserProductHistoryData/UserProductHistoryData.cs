using System.Net;
using FairWearGateway.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Users.Service;

namespace FairWearGateway.API.DataAccess.UserProductHistoryData;

/// <summary>
/// Data access class for user product history data operations.
/// </summary>
public class UserProductHistoryData : IUserProductHistoryData
{
    private readonly UserProductHistoryService.UserProductHistoryServiceClient _client;

    /// <summary>Constructor</summary>
    public UserProductHistoryData(GrpcClientFactory grpcClientFactory)
    {
        _client =
            grpcClientFactory.CreateClient<UserProductHistoryService.UserProductHistoryServiceClient>("UserProductHistoryService");
    }
    
    /// <inheritdoc />
    public ProcessingStatusResponse<GetUserProductHistoryResponse> GetUserProductHistoryByUserId(long userId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<GetUserProductHistoryResponse>();

        var data = new UserProductHistoryRequest() { Id = userId };

        try
        {
            var response = _client.GetUserProductHistory(data);
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
    public ProcessingStatusResponse<UserProductHistory> CreateUserProductHistory(UserProductHistory request)
    {
        var processingStatusResponse = new ProcessingStatusResponse<UserProductHistory>();

        try
        {
            var response = _client.AddUserProductHistory(request);
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
    public ProcessingStatusResponse<UserProductHistory> UpdateUserProductHistory(UserProductHistory request)
    {
        var processingStatusResponse = new ProcessingStatusResponse<UserProductHistory>();
        try
        {
            var response = _client.UpdateUserProductHistory(request);
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
    public ProcessingStatusResponse<Empty> DeleteUserProductHistory(long userId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<Empty>();

        var data = new UserProductHistoryRequest() { Id = userId };

        try
        {
            var response = _client.DeleteUserProductHistory(data);
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