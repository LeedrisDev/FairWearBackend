using System.Net;
using FairWearProductDataRetriever.Service.Business.ProductBusiness;
using FairWearProductDataRetriever.Service.Protos;
using Grpc.Core;

namespace FairWearProductDataRetriever.Service.Services;

/// <summary>
/// Service for retrieving product information.
/// </summary>
public class ProductSrapperService : Protos.ProductScrapperService.ProductScrapperServiceBase
{
    private readonly ILogger<ProductSrapperService> _logger;
    private readonly IProductBusiness _productBusiness;

    /// <summary>Constructor</summary>
    /// <param name="productBusiness"> Product business.</param>
    /// <param name="logger"> logger.</param>
    public ProductSrapperService(IProductBusiness productBusiness, ILogger<ProductSrapperService> logger)
    {
        _productBusiness = productBusiness;
        _logger = logger;
    }

    /// <summary> Get product information from a barcode.</summary>
    /// <param name="request">Object containing barcode of the product.</param>
    /// <param name="context">gRPC context</param>
    /// <returns> Product information.</returns>
    public override async Task<ProductScrapperResponse> GetProduct(ProductScrapperRequest request,
        ServerCallContext context)
    {
        var productInformation = await _productBusiness.GetProductInformation(request.UpcCode);

        return productInformation.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                productInformation.ErrorMessage)),
            HttpStatusCode.OK => productInformation.Object,
            _ => throw new RpcException(new Status(StatusCode.Internal, productInformation.ErrorMessage))
        };
    }
}