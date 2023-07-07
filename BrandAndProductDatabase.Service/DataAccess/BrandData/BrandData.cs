using System.Net;
using AutoMapper;
using BrandAndProductDatabase.Service.Models;
using BrandAndProductDatabase.Service.Models.Dto;
using BrandAndProductDatabase.Service.Utils;
using GoodOnYouScrapper.Service.Protos;
using Grpc.Core;
using Grpc.Net.Client;

namespace BrandAndProductDatabase.Service.DataAccess.BrandData;

/// <summary>Class to contact appropriate microservice for brand data.</summary>
public class BrandData : IBrandData
{
    private readonly GrpcChannel _channel;
    private readonly BrandScrapperService.BrandScrapperServiceClient _client;
    private readonly IMapper _mapper;

    /// <summary>Constructor</summary>
    public BrandData(IMapper mapper)
    {
        _channel = GrpcChannel.ForAddress(AppConstants.GoodOnYouScrapperUrl);
        _client = new BrandScrapperService.BrandScrapperServiceClient(_channel);
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