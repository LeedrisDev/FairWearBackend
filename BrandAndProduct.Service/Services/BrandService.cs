using System.Net;
using AutoMapper;
using BrandAndProduct.Service.Business.BrandBusiness;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BrandAndProduct.Service.Services;

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

    public override async Task GetAllBrandsAsync(BrandFilterList request,
        IServerStreamWriter<BrandResponse> responseStream,
        ServerCallContext context)
    {
        var filters = new Dictionary<string, string>();

        foreach (BrandFilter filter in request.Filters)
        {
            filters.Add(filter.Key, filter.Value);
        }

        var brandList = await _brandBusiness.GetAllBrandsAsync(filters);

        if (brandList.Status != HttpStatusCode.OK)
        {
            _logger.LogError("Error while retrieving brands: {ErrorMessage}", brandList.ErrorMessage);
            throw new RpcException(new Status(StatusCode.Internal, brandList.ErrorMessage));
        }

        foreach (var brand in brandList.Object)
        {
            await responseStream.WriteAsync(_mapper.Map<BrandResponse>(brand));
        }
    }

    public override async Task<BrandResponse> GetBrandByIdAsync(BrandByIdRequest request, ServerCallContext context)
    {
        var brand = await _brandBusiness.GetBrandByIdAsync(request.Id);
        if (brand.Status != HttpStatusCode.OK)
            _logger.LogError("Error while retrieving brand: {ErrorMessage}", brand.ErrorMessage);

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
        if (brand.Status != HttpStatusCode.OK)
            _logger.LogError("Error while retrieving brand: {ErrorMessage}", brand.ErrorMessage);

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
        if (createdBrand.Status != HttpStatusCode.OK)
            _logger.LogError("Error while creating brand: {ErrorMessage}", createdBrand.ErrorMessage);
        return createdBrand.Status switch
        {
            HttpStatusCode.OK => _mapper.Map<BrandResponse>(createdBrand.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, createdBrand.ErrorMessage))
        };
    }

    public override async Task<BrandResponse> UpdateBrandAsync(BrandResponse request, ServerCallContext context)
    {
        var updatedBrand = await _brandBusiness.UpdateBrandAsync(_mapper.Map<BrandDto>(request));
        if (updatedBrand.Status != HttpStatusCode.OK)
            _logger.LogError("Error while updating brands: {ErrorMessage}", updatedBrand.ErrorMessage);
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
            _logger.LogError("Error while deleting brands: {ErrorMessage}", brand.ErrorMessage);
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