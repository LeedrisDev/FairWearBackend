using System.Net;
using GoodOnYouScrapper.Service.Business.BrandBusiness;
using GoodOnYouScrapper.Service.Protos;
using Grpc.Core;

namespace GoodOnYouScrapper.Service.Services;

/// <summary>
/// Service for retrieving brand information.
/// </summary>
public class BrandScrapperService : Protos.BrandScrapperService.BrandScrapperServiceBase
{
    private readonly IBrandBusiness _brandBusiness;
    private readonly ILogger<BrandScrapperService> _logger;

    /// <summary>Constructor</summary>
    /// <param name="brandBusiness"> Brand business.</param>
    /// <param name="logger">logger.</param>
    public BrandScrapperService(IBrandBusiness brandBusiness, ILogger<BrandScrapperService> logger)
    {
        _brandBusiness = brandBusiness;
        _logger = logger;
    }

    /// <summary>Get brand information</summary>
    /// <param name="request">Object containing the brand name</param>
    /// <returns> Brand information </returns>
    public override async Task<BrandScrapperResponse> GetBrand(BrandScrapperRequest request, ServerCallContext context)
    {
        var brandInformation = await _brandBusiness.GetBrandInformation(request.Name);

        return brandInformation.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                brandInformation.ErrorMessage)),
            HttpStatusCode.OK => brandInformation.Object,
            _ => throw new RpcException(new Status(StatusCode.Internal, brandInformation.ErrorMessage))
        };
    }
}