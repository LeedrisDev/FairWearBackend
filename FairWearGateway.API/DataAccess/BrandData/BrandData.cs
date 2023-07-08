using System.Net;
using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.Models;
using Grpc.Core;
using Grpc.Net.ClientFactory;

namespace FairWearGateway.API.DataAccess.BrandData;

/// <summary>Class that call the appropriate microservice to get all is related to brands.</summary>
public class BrandData : IBrandData
{
    private readonly BrandService.BrandServiceClient _client;

    /// <summary>Constructor</summary>
    public BrandData(GrpcClientFactory grpcClientFactory)
    {
        _client = grpcClientFactory.CreateClient<BrandService.BrandServiceClient>("BrandService");
    }

    /// <inheritdoc />
    public ProcessingStatusResponse<BrandResponse> GetBrandById(int brandId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<BrandResponse>();

        var data = new BrandByIdRequest { Id = brandId };

        try
        {
            var response = _client.GetBrandByIdAsync(data);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = $"Brand with id {brandId} could not be found";
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
    public ProcessingStatusResponse<BrandResponse> GetBrandByName(string name)
    {
        var processingStatusResponse = new ProcessingStatusResponse<BrandResponse>();

        var data = new BrandByNameRequest { Name = name };

        try
        {
            var response = _client.GetBrandByNameAsync(data);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = $"Brand with name {name} could not be found";
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