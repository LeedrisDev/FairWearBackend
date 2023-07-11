using System.Net;
using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.Models;
using FairWearGateway.API.Utils;
using Grpc.Core;
using Grpc.Net.Client;

namespace FairWearGateway.API.DataAccess.ProductData;

/// <summary>Class that implements the <see cref="IProductData"/> interface.</summary>
public class ProductData : IProductData
{
    private readonly ProductService.ProductServiceClient _client;

    /// <summary>Constructor</summary>
    public ProductData()
    {
        var channel = GrpcChannel.ForAddress(AppConstants.BrandAndProductServiceUrl);
        _client = new ProductService.ProductServiceClient(channel);
    }

    /// <inheritdoc />
    public ProcessingStatusResponse<ProductResponse> GetProductById(int productId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<ProductResponse>();

        var data = new ProductByIdRequest { Id = productId };

        try
        {
            var response = _client.GetProductByIdAsync(data);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = $"Product with id {productId} could not be found";
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
    public ProcessingStatusResponse<ProductInformationResponse> GetProductByUpc(string upc)
    {
        var processingStatusResponse = new ProcessingStatusResponse<ProductInformationResponse>();

        var data = new ProductByUpcRequest { UpcCode = upc };

        try
        {
            var response = _client.GetProductByUpcAsync(data);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = response;
        }
        catch (RpcException e)
        {
            if (e.Status.StatusCode == StatusCode.NotFound)
            {
                processingStatusResponse.Status = HttpStatusCode.NotFound;
                processingStatusResponse.ErrorMessage = e.Status.Detail;
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
    public async Task<ProcessingStatusResponse<IEnumerable<ProductResponse>>> GetAllProducts(
        Dictionary<string, string> filters)
    {
        var processingStatusResponse = new ProcessingStatusResponse<IEnumerable<ProductResponse>>();

        var data = new ProductFilterList();

        foreach (KeyValuePair<string, string> kvp in filters)
        {
            data.Filters.Add(new ProductFilter { Key = kvp.Key, Value = kvp.Value });
        }

        try
        {
            var response = _client.GetAllProductsAsync(data);
            var brandList = new List<ProductResponse>();
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