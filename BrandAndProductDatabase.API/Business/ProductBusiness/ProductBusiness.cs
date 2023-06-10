using System.Net;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;

namespace BrandAndProductDatabase.API.Business.ProductBusiness;

/// <summary>Logic for product repository operations</summary>
public class ProductBusiness : IProductBusiness
{
    private readonly IBrandRepository _brandRepository;
    private readonly IProductRepository _productRepository;

    /// <summary>Constructor for ProductBusiness.</summary>
    /// <param name="productRepository"></param>
    /// <param name="brandRepository"></param>
    public ProductBusiness(IProductRepository productRepository, IBrandRepository brandRepository)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
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
        var productDataResponse = await _productRepository.GetAllAsync();

        if (productDataResponse.Status != HttpStatusCode.OK)
        {
            return new ProcessingStatusResponse<ProductInformationDto>()
            {
                Status = productDataResponse.Status,
                ErrorMessage = productDataResponse.ErrorMessage
            };
        }

        var product = productDataResponse.Object.FirstOrDefault(x => x.UpcCode == upcCode);

        if (product == null)
        {
            return new ProcessingStatusResponse<ProductInformationDto>()
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = $"Product with UpcCode {upcCode} does not exist."
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

        var productScore = new ProductScoreDto()
        {
            Moral = brand.PeopleRating,
            Animal = brand.AnimalRating,
            Environmental = brand.EnvironmentRating
        };

        return new ProcessingStatusResponse<ProductInformationDto>()
        {
            Object = new ProductInformationDto()
            {
                Name = product.Name,
                Country = brand.Country,
                Image = "No image found",
                Scores = productScore,
                GlobalScore = (productScore.Moral + productScore.Animal + productScore.Environmental) / 3,
                Composition = Array.Empty<ProductCompositionDto>(),
                Alternatives = Array.Empty<string>(),
                Brand = brand.Name
            }
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
}