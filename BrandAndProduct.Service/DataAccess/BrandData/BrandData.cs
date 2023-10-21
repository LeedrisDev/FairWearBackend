using System.Net;
using AutoMapper;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;
using GoodOnYouScrapper.Service.Protos;
using Grpc.Core;
using Grpc.Net.ClientFactory;

namespace BrandAndProduct.Service.DataAccess.BrandData;

/// <summary>Class to contact appropriate microservice for brand data.</summary>
public class BrandData : IBrandData
{
    private readonly BrandScrapperService.BrandScrapperServiceClient _client;
    private readonly IMapper _mapper;

    /// <summary>Constructor</summary>
    public BrandData(GrpcClientFactory grpcClientFactory, IMapper mapper)
    {
        _client = grpcClientFactory.CreateClient<BrandScrapperService.BrandScrapperServiceClient>("BrandService");
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<BrandDto> GetBrandByName(string name)
    {
        var processingStatusResponse = new ProcessingStatusResponse<BrandDto>();

        var data = new BrandScrapperRequest { Name = name };

        try
        {
            var response = _client.GetBrand(data);
            processingStatusResponse.Status = HttpStatusCode.OK;
            processingStatusResponse.Object = _mapper.Map<BrandDto>(response);
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