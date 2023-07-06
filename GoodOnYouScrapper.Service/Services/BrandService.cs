using Grpc.Core;
using System.Net;
using GoodOnYouScrapper.Service.Business.BrandBusiness;
using GoodOnYouScrapper.Service.Proto;

namespace GoodOnYouScrapper.Service.Services
{
    public class BrandService : Proto.BrandService.BrandServiceBase
    {
        private readonly ILogger<BrandService> _logger;
        private readonly IBrandBusiness _brandBusiness;

        public BrandService(IBrandBusiness brandBusiness, ILogger<BrandService> logger)
        {
            _brandBusiness = brandBusiness;
            _logger = logger;
        }

        public override async Task<BrandResponse> GetBrand(BrandRequest request, ServerCallContext context)
        {
            var brandInformation = await _brandBusiness.GetBrandInformation(request.Name);

            return brandInformation.Status switch
            {
                HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound, brandInformation.ErrorMessage)),
                HttpStatusCode.OK => brandInformation.Object,
                _ => throw new RpcException(new Status(StatusCode.Internal, brandInformation.ErrorMessage))
            };
        }
        
    }
}
