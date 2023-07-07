using System.Net;
using AutoMapper;
using BrandAndProductDatabase.Service.Models;
using BrandAndProductDatabase.Service.Utils;
using FairWearProductDataRetriever.Service.Protos;
using Grpc.Core;
using Grpc.Net.Client;

namespace BrandAndProductDatabase.Service.DataAccess.ProductData;

/// <summary>Class to contact appropriate microservice for product data.</summary>
public class ProductData : IProductData
{
    private readonly GrpcChannel _channel;
    private readonly ProductScrapperService.ProductScrapperServiceClient _client;
    private readonly IMapper _mapper;


    /// <summary>Constructor.</summary>
    public ProductData(IMapper mapper)
    {
        _channel = GrpcChannel.ForAddress(AppConstants.ProductDataRetrieverUrl);
        _client = new ProductScrapperService.ProductScrapperServiceClient(_channel);
        _mapper = mapper;
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
                processingStatusResponse.ErrorMessage = e.Message;
            }
            else
            {
                processingStatusResponse.Status = HttpStatusCode.InternalServerError;
                processingStatusResponse.ErrorMessage = e.Message;
            }
        }

        return processingStatusResponse;
    }
}