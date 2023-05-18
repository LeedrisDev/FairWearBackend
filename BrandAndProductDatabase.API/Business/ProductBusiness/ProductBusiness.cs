using System.Net;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.DataAccess.ProductData;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;

namespace BrandAndProductDatabase.API.Business.ProductBusiness;

/// <summary>Logic for product repository operations</summary>
public class ProductBusiness : IProductBusiness
{
    private readonly IBrandRepository _brandRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductData _productData;

    /// <summary>Constructor for ProductBusiness.</summary>
    /// <param name="productRepository"></param>
    /// <param name="brandRepository"></param>
    /// <param name="productData"></param>
    public ProductBusiness(IProductRepository productRepository, IBrandRepository brandRepository, IProductData productData)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _productData = productData;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> GetProductByBarcodeAsync(string barcode)
    {
        var repositoryResponse = await _productRepository.GetProductByBarcodeAsync(barcode);
        if (repositoryResponse.Status == HttpStatusCode.OK)
            return repositoryResponse;
        
        var productDataResponse = await _productData.GetProductByBarcode(barcode);
        var entityFromDatabase = await _productRepository.AddAsync(productDataResponse.Object);
        return entityFromDatabase;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> CreateProductAsync(ProductDto productDto)
    {
        var brandExists = await _brandRepository.GetByIdAsync(productDto.BrandId);
        if (brandExists.Status == HttpStatusCode.NotFound)
        {
            return new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.BadRequest,
                ErrorMessage = $"Brand with Id {productDto.BrandId} does not exist."
            };
        }

        return await _productRepository.AddAsync(productDto);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> UpdateProductAsync(ProductDto productDto)
    {
        var brandExists = await _brandRepository.GetByIdAsync(productDto.BrandId);
        if (brandExists.Status == HttpStatusCode.NotFound)
        {
            return new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.BadRequest,
                ErrorMessage = $"Brand with Id {productDto.BrandId} does not exist."
            };
        }

        return await _productRepository.UpdateAsync(productDto);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> DeleteProductAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}