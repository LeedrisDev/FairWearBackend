using System.Net;
using AutoMapper;
using BrandAndProduct.Service.Business.ProductBusiness;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BrandAndProduct.Service.Services;

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

    public override async Task GetAllProductsAsync(ProductFilterList request,
        IServerStreamWriter<ProductResponse> responseStream,
        ServerCallContext context)
    {
        var filters = new Dictionary<string, string>();

        foreach (ProductFilter filter in request.Filters)
        {
            filters.Add(filter.Key, filter.Value);
        }

        var productList = await _productBusiness.GetAllProductsAsync(filters);

        if (productList.Status != HttpStatusCode.OK)
        {
            _logger.LogError("Error while retrieving products: {ErrorMessage}", productList.ErrorMessage);
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

        if (product.Status != HttpStatusCode.OK)
            _logger.LogError("Error while retrieving product: {ErrorMessage}", product.ErrorMessage);

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

        if (product.Status != HttpStatusCode.OK)
            _logger.LogError("Error while retrieving product: {ErrorMessage}", product.ErrorMessage);

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

        if (createdProduct.Status != HttpStatusCode.OK)
            _logger.LogError("Error while creating product: {ErrorMessage}", createdProduct.ErrorMessage);

        return createdProduct.Status switch
        {
            HttpStatusCode.OK => _mapper.Map<ProductResponse>(createdProduct.Object),
            _ => throw new RpcException(new Status(StatusCode.Internal, createdProduct.ErrorMessage))
        };
    }

    public override async Task<ProductResponse> UpdateProductAsync(ProductResponse request, ServerCallContext context)
    {
        var updatedProduct = await _productBusiness.UpdateProductAsync(_mapper.Map<ProductDto>(request));

        if (updatedProduct.Status != HttpStatusCode.OK)
            _logger.LogError("Error while updating product: {ErrorMessage}", updatedProduct.ErrorMessage);

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
            _logger.LogError("Error while deleting product: {ErrorMessage}", product.ErrorMessage);

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