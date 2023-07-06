using System.Net;
using FairWearProductDataRetriever.Service.Business.ProductBusiness;
using FairWearProductDataRetriever.Service.Protos;
using Grpc.Core;

namespace FairWearProductDataRetriever.Service.Services
{
    public class ProductService : Protos.ProductService.ProductServiceBase
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductBusiness _productBusiness;

        public ProductService(IProductBusiness productBusiness, ILogger<ProductService> logger)
        {
            _productBusiness = productBusiness;
            _logger = logger;
        }

        public override async Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
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
}