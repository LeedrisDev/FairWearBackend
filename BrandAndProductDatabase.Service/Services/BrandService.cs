using System.Net;
using AutoMapper;
using BrandAndProductDatabase.Service.Business.BrandBusiness;
using BrandAndProductDatabase.Service.Models.Dto;
using BrandAndProductDatabase.Service.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BrandAndProductDatabase.Service.Services;

public class BrandService : Protos.BrandService.BrandServiceBase
{
    private readonly IBrandBusiness _brandBusiness;
    private readonly ILogger<BrandService> _logger;
    private readonly IMapper _mapper;


    public BrandService(IBrandBusiness brandBusiness, ILogger<BrandService> logger, IMapper mapper)
    {
        _brandBusiness = brandBusiness;
        _logger = logger;
        _mapper = mapper;
    }

    public override async Task GetAllBrandsAsync(Empty request, IServerStreamWriter<BrandResponse> responseStream,
        ServerCallContext context)
    {
        var brandList = await _brandBusiness.GetAllBrandsAsync();

        if (brandList.Status != HttpStatusCode.OK)
        {
            throw new RpcException(new Status(StatusCode.Internal, brandList.ErrorMessage));
        }

        foreach (BrandDto brand in brandList.Object)
        {
            await responseStream.WriteAsync(_mapper.Map<BrandResponse>(brand));
        }
    }

    public override async Task<BrandResponse> GetBrandByIdAsync(BrandByIdRequest request, ServerCallContext context)
    {
        var brand = await _brandBusiness.GetBrandByIdAsync(request.Id);

        return brand.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound, brand.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<BrandResponse>(brand.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, brand.ErrorMessage))
        };
    }

    public override async Task<BrandResponse> GetBrandByNameAsync(BrandByNameRequest request,
        ServerCallContext context)
    {
        var brand = await _brandBusiness.GetBrandByNameAsync(request.Name);

        return brand.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound, brand.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<BrandResponse>(brand.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, brand.ErrorMessage))
        };
    }

    public override async Task<BrandResponse> CreateBrandAsync(BrandRequest request, ServerCallContext context)
    {
        var createdBrand = await _brandBusiness.CreateBrandAsync(_mapper.Map<BrandDto>(request));

        return createdBrand.Status switch
        {
            HttpStatusCode.OK => _mapper.Map<BrandResponse>(createdBrand.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, createdBrand.ErrorMessage))
        };
    }

    public override async Task<BrandResponse> UpdateBrandAsync(BrandResponse request, ServerCallContext context)
    {
        var updatedBrand = await _brandBusiness.UpdateBrandAsync(_mapper.Map<BrandDto>(request));

        return updatedBrand.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                updatedBrand.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<BrandResponse>(updatedBrand.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, updatedBrand.ErrorMessage))
        };
    }

    public override async Task<Empty> DeleteBrandAsync(BrandByIdRequest request, ServerCallContext context)
    {
        var brand = await _brandBusiness.GetBrandByIdAsync(request.Id);

        if (brand.Status != HttpStatusCode.OK)
        {
            return brand.Status switch
            {
                HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                    brand.ErrorMessage)),
                _ => throw new RpcException(new Status(StatusCode.Internal, brand.ErrorMessage))
            };
        }

        var deleteBrand = await _brandBusiness.DeleteBrandAsync(request.Id);

        return deleteBrand.Status switch
        {
            HttpStatusCode.OK => new Empty(),
            _ => throw new RpcException(new Status(StatusCode.Internal, brand.ErrorMessage))
        };
    }
}