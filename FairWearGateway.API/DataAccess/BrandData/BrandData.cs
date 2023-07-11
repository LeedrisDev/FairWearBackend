using System.Net;
using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.Models;
using FairWearGateway.API.Utils;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;

namespace FairWearGateway.API.DataAccess.BrandData;

/// <summary>Class that call the appropriate microservice to get all is related to brands.</summary>
public class BrandData : IBrandData
{
    private readonly BrandService.BrandServiceClient _client;

    /// <summary>Constructor</summary>
    public BrandData()
    {
        var channel = GrpcChannel.ForAddress(AppConstants.BrandAndProductServiceUrl);
        _client = new BrandService.BrandServiceClient(channel);
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

    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<IEnumerable<BrandResponse>>> GetAllBrands(
        Dictionary<string, string> filters)
    {
        var processingStatusResponse = new ProcessingStatusResponse<IEnumerable<BrandResponse>>();

        var data = new BrandFilterList();

        foreach (KeyValuePair<string, string> kvp in filters)
        {
            data.Filters.Add(new BrandFilter { Key = kvp.Key, Value = kvp.Value });
        }

        try
        {
            var response = _client.GetAllBrandsAsync(data);
            var brandList = new List<BrandResponse>();
            while (await response.ResponseStream.MoveNext())
            {
                brandList.Add(response.ResponseStream.Current);
            }

            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = brandList;
        }
        catch (RpcException e)
        {
            processingStatusResponse.Status = HttpStatusCode.InternalServerError;
            processingStatusResponse.ErrorMessage = e.Status.Detail;
        }

        return processingStatusResponse;
    }
}