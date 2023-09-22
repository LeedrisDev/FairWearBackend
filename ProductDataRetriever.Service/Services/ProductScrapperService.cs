using System.Net;
using ProductDataRetriever.Service.Protos;
using Grpc.Core;
using ProductDataRetriever.Service.Business.ProductBusiness;

namespace ProductDataRetriever.Service.Services;

/// <summary>
/// Service for retrieving product information.
/// </summary>
public class ProductScrapperService : Protos.ProductScrapperService.ProductScrapperServiceBase
{
    private readonly IProductBusiness _productBusiness;

    /// <summary>Constructor</summary>
    /// <param name="productBusiness"> Product business.</param>
    public ProductScrapperService(IProductBusiness productBusiness)
    {
        _productBusiness = productBusiness;
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