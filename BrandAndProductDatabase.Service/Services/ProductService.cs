using System.Net;
using AutoMapper;
using BrandAndProductDatabase.Service.Business.ProductBusiness;
using BrandAndProductDatabase.Service.Models.Dto;
using BrandAndProductDatabase.Service.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BrandAndProductDatabase.Service.Services;

public class ProductService : Protos.ProductService.ProductServiceBase
{
    private readonly ILogger<ProductService> _logger;
    private readonly IMapper _mapper;
    private readonly IProductBusiness _productBusiness;

    public ProductService(IProductBusiness productBusiness, ILogger<ProductService> logger, IMapper mapper)
    {
        _productBusiness = productBusiness;
        _logger = logger;
        _mapper = mapper;
    }

    public override async Task GetAllProductsAsync(Empty request, IServerStreamWriter<ProductResponse> responseStream,
        ServerCallContext context)
    {
        var productList = await _productBusiness.GetAllProductsAsync(new Dictionary<string, string>());

        if (productList.Status != HttpStatusCode.OK)
        {
            throw new RpcException(new Status(StatusCode.Internal, productList.ErrorMessage));
        }

        foreach (ProductDto product in productList.Object)
        {
            await responseStream.WriteAsync(_mapper.Map<ProductResponse>(product));
        }
    }

    public override async Task<ProductResponse> GetProductByIdAsync(ProductByIdRequest request,
        ServerCallContext context)
    {
        var product = await _productBusiness.GetProductByIdAsync(request.Id);

        return product.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound, product.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<ProductResponse>(product.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, product.ErrorMessage))
        };
    }

    public override async Task<ProductInformationResponse> GetProductByUpcAsync(ProductByUpcRequest request,
        ServerCallContext context)
    {
        var product = await _productBusiness.GetProductByUpcAsync(request.UpcCode);

        return product.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound, product.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<ProductInformationResponse>(product.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, product.ErrorMessage))
        };
    }

    public override async Task<ProductResponse> CreateProductAsync(ProductRequest request, ServerCallContext context)
    {
        var createdProduct = await _productBusiness.CreateProductAsync(_mapper.Map<ProductDto>(request));

        return createdProduct.Status switch
        {
            HttpStatusCode.OK => _mapper.Map<ProductResponse>(createdProduct.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, createdProduct.ErrorMessage))
        };
    }

    public override async Task<ProductResponse> UpdateProductAsync(ProductResponse request, ServerCallContext context)
    {
        var updatedProduct = await _productBusiness.UpdateProductAsync(_mapper.Map<ProductDto>(request));

        return updatedProduct.Status switch
        {
            HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                updatedProduct.ErrorMessage)),
            HttpStatusCode.OK => _mapper.Map<ProductResponse>(updatedProduct.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, updatedProduct.ErrorMessage))
        };
    }

    public override async Task<Empty> DeleteProductAsync(ProductByIdRequest request, ServerCallContext context)
    {
        var product = await _productBusiness.GetProductByIdAsync(request.Id);

        if (product.Status != HttpStatusCode.OK)
        {
            return product.Status switch
            {
                HttpStatusCode.NotFound => throw new RpcException(new Status(StatusCode.NotFound,
                    product.ErrorMessage)),
                _ => throw new RpcException(new Status(StatusCode.Internal, product.ErrorMessage))
            };
        }

        var deleteProduct = await _productBusiness.DeleteProductAsync(request.Id);

        return deleteProduct.Status switch
        {
            HttpStatusCode.OK => new Empty(),
            _ => throw new RpcException(new Status(StatusCode.Internal, product.ErrorMessage))
        };
    }
}