using System.Net;
using BrandAndProduct.Service.Models;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using ProductDataRetriever.Service.Protos;

namespace BrandAndProduct.Service.DataAccess.ProductData;

/// <summary>Class to contact appropriate microservice for product data.</summary>
public class ProductData : IProductData
{
    private readonly ProductScrapperService.ProductScrapperServiceClient _client;

    /// <summary>Constructor.</summary>
    public ProductData(GrpcClientFactory grpcClientFactory)
    {
        _client = grpcClientFactory.CreateClient<ProductScrapperService.ProductScrapperServiceClient>("ProductService");
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<ProductScrapperResponse> GetProductByUpc(string upc)
    {
        var processingStatusResponse = new ProcessingStatusResponse<ProductScrapperResponse>();

        var data = new ProductScrapperRequest { UpcCode = upc };

        try
        {
            var response = _client.GetProduct(data);
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
}