using System.Net;
using BrandAndProductDatabase.Service.DataAccess.BrandData;
using BrandAndProductDatabase.Service.DataAccess.IRepositories;
using BrandAndProductDatabase.Service.DataAccess.ProductData;
using BrandAndProductDatabase.Service.Models;
using BrandAndProductDatabase.Service.Models.Dto;

namespace BrandAndProductDatabase.Service.Business.ProductBusiness;

/// <summary>Logic for product repository operations</summary>
public class ProductBusiness : IProductBusiness
{
    private readonly IBrandData _brandData;
    private readonly IBrandRepository _brandRepository;
    private readonly IProductData _productData;
    private readonly IProductRepository _productRepository;

    /// <summary>Constructor for ProductBusiness.</summary>
    /// <param name="productRepository"></param>
    /// <param name="brandRepository"></param>
    /// <param name="productData"></param>
    /// <param name="brandData"></param>
    public ProductBusiness(IProductRepository productRepository, IBrandRepository brandRepository,
        IProductData productData, IBrandData brandData)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _productData = productData;
        _brandData = brandData;
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

    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<ProductInformationDto>> GetProductByUpcAsync(string upcCode)
    {
        var productResponse = await _productRepository.GetAllAsync();

        if (productResponse.Status != HttpStatusCode.OK)
        {
            return new ProcessingStatusResponse<ProductInformationDto>()
            {
                Status = productResponse.Status,
                ErrorMessage = productResponse.ErrorMessage
            };
        }

        var product = productResponse.Object.FirstOrDefault(x => x.UpcCode == upcCode);

        if (product == null)
        {
            var productDataResponse = _productData.GetProductByUpc(upcCode);

            if (productDataResponse.Status != HttpStatusCode.OK)
            {
                return new ProcessingStatusResponse<ProductInformationDto>()
                {
                    Status = HttpStatusCode.NotFound,
                    ErrorMessage = $"Product with UpcCode {upcCode} does not exist."
                };
            }

            var productBrand = _brandData.GetBrandByName(productDataResponse.Object.BrandName);

            if (productBrand.Status != HttpStatusCode.OK)
            {
                return new ProcessingStatusResponse<ProductInformationDto>()
                {
                    Status = productBrand.Status,
                    ErrorMessage = productBrand.ErrorMessage
                };
            }

            productBrand = await _brandRepository.AddAsync(productBrand.Object);

            var entityFromDatabase = await _productRepository.AddAsync(
                new ProductDto()
                {
                    Name = productDataResponse.Object.Name,
                    UpcCode = upcCode,
                    Category = productDataResponse.Object.Category,
                    Ranges = productDataResponse.Object.Ranges.ToList(),
                    BrandId = productBrand.Object.Id
                });

            return new ProcessingStatusResponse<ProductInformationDto>()
            {
                Object = ProductDtoToProductInformation(entityFromDatabase.Object, productBrand.Object)
            };
        }

        var brandDataResponse = await _brandRepository.GetByIdAsync(product.BrandId);

        if (brandDataResponse.Status != HttpStatusCode.OK)
        {
            return new ProcessingStatusResponse<ProductInformationDto>()
            {
                Status = brandDataResponse.Status,
                ErrorMessage = brandDataResponse.ErrorMessage
            };
        }

        var brand = brandDataResponse.Object;

        return new ProcessingStatusResponse<ProductInformationDto>()
        {
            Object = ProductDtoToProductInformation(product, brand)
        };
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

    private ProductInformationDto ProductDtoToProductInformation(ProductDto productDto, BrandDto brandDto)
    {
        var productScore = new ProductScoreDto()
        {
            Moral = brandDto.PeopleRating,
            Animal = brandDto.AnimalRating,
            Environmental = brandDto.EnvironmentRating
        };

        var brandInformation = new ProductInformationDto()
        {
            Name = productDto.Name,
            Country = brandDto.Country,
            Image = "No image found",
            Scores = productScore,
            GlobalScore = (productScore.Moral + productScore.Animal + productScore.Environmental) / 3,
            Composition = new List<ProductCompositionDto>(),
            Brand = brandDto.Name
        };

        return brandInformation;
    }
}