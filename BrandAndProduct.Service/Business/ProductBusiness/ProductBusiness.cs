using System.Net;
using BrandAndProduct.Service.DataAccess.BrandData;
using BrandAndProduct.Service.DataAccess.Filters;
using BrandAndProduct.Service.DataAccess.IRepositories;
using BrandAndProduct.Service.DataAccess.ProductData;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Services;
using Newtonsoft.Json;

namespace BrandAndProduct.Service.Business.ProductBusiness;

/// <summary>Logic for product repository operations</summary>
public class ProductBusiness : IProductBusiness
{
    private readonly IBrandData _brandData;
    private readonly IBrandRepository _brandRepository;
    private readonly IFilterFactory<IFilter> _filterFactory;
    private readonly IIntegrationEventRepository _integrationEventRepository;
    private readonly IIntegrationEventSenderService _integrationEventSenderService;
    private readonly IProductData _productData;
    private readonly IProductRepository _productRepository;

    /// <summary>Constructor for ProductBusiness.</summary>
    /// <param name="productRepository"></param>
    /// <param name="brandRepository"></param>
    /// <param name="productData"></param>
    /// <param name="brandData"></param>
    /// <param name="filterFactory"></param>
    /// <param name="integrationEventRepository"></param>
    /// <param name="integrationEventSenderService"></param>
    public ProductBusiness(IProductRepository productRepository, IBrandRepository brandRepository,
        IProductData productData, IBrandData brandData, IFilterFactory<IFilter> filterFactory,
        IIntegrationEventRepository integrationEventRepository,
        IIntegrationEventSenderService integrationEventSenderService)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _productData = productData;
        _brandData = brandData;
        _filterFactory = filterFactory;
        _integrationEventSenderService = integrationEventSenderService;
        _integrationEventRepository = integrationEventRepository;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<ProductDto>>> GetAllProductsAsync(
        Dictionary<string, string> filters)
    {
        var filter = _filterFactory.CreateFilter(filters);
        return await _productRepository.GetAllAsync(filter);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<ProductInformationDto>> GetProductByUpcAsync(string upcCode)
    {
        var filterDict = new Dictionary<string, string> { { "UpcCode", upcCode } };

        var upcFilter = _filterFactory.CreateFilter(filterDict);

        var productResponse = await _productRepository.GetAllAsync(upcFilter);

        if (productResponse.Status != HttpStatusCode.OK)
        {
            return new ProcessingStatusResponse<ProductInformationDto>()
            {
                Status = productResponse.Status,
                ErrorMessage = productResponse.ErrorMessage
            };
        }

        if (productResponse.Object.ToList().Count == 0)
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

            var brandEntity = await _brandRepository.GetBrandByNameAsync(productDataResponse.Object.BrandName);

            if (brandEntity == null || brandEntity.Status != HttpStatusCode.OK)
            {
                var treatedName = productDataResponse.Object.BrandName
                    .ToLower()
                    .Replace(" ", "-")
                    .Replace("'", "");

                var productBrand = _brandData.GetBrandByName(treatedName);

                if (productBrand.Status != HttpStatusCode.OK)
                {
                    return new ProcessingStatusResponse<ProductInformationDto>()
                    {
                        Status = productBrand.Status,
                        ErrorMessage = productBrand.ErrorMessage
                    };
                }
                
                brandEntity = await _brandRepository.AddAsync(productBrand.Object);
            }

            var entityFromDatabase = await _productRepository.AddAsync(
                new ProductDto()
                {
                    Name = productDataResponse.Object.Name,
                    UpcCode = upcCode,
                    Category = productDataResponse.Object.Category,
                    Ranges = productDataResponse.Object.Ranges.ToList(),
                    BrandId = brandEntity.Object.Id
                });

            var response = new ProcessingStatusResponse<ProductInformationDto>()
            {
                Object = ProductDtoToProductInformation(entityFromDatabase.Object, brandEntity.Object)
            };

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = entityFromDatabase.Object.Id,
                name = response.Object.Name,
                rating = response.Object.GlobalScore,
            });

            await PublishToMessageQueue("product.create", integrationEventData);

            _integrationEventSenderService.StartPublishingOutstandingIntegrationEvents();

            return response;
        }

        var product = productResponse.Object.First();

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

        var product = await _productRepository.AddAsync(productDto);

        if (product.Status == HttpStatusCode.OK)
        {
            var productInformation = ProductDtoToProductInformation(product.Object, brandExists.Object);

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = product.Object.Id,
                name = productInformation.Name,
                rating = productInformation.GlobalScore,
            });

            await PublishToMessageQueue("product.create", integrationEventData);

            _integrationEventSenderService.StartPublishingOutstandingIntegrationEvents();
        }

        return product;
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

        var product = await _productRepository.UpdateProductAsync(productDto);
        if (product.Status == HttpStatusCode.OK)
        {
            var productInformation = ProductDtoToProductInformation(product.Object, brandExists.Object);

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = product.Object.Id,
                name = productInformation.Name,
                rating = productInformation.GlobalScore,
            });

            await PublishToMessageQueue("product.update", integrationEventData);

            _integrationEventSenderService.StartPublishingOutstandingIntegrationEvents();
        }

        return product;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> DeleteProductAsync(int id)
    {
        var response = await _productRepository.DeleteAsync(id);

        if (response.Status == HttpStatusCode.OK)
        {
            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = id,
            });

            PublishToMessageQueue("product.delete", integrationEventData);

            _integrationEventSenderService.StartPublishingOutstandingIntegrationEvents();
        }

        return response;
    }

    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<IEnumerable<ProductDto>>> GetProductAlternativesAsync(int productId)
    {
        return await _productRepository.GetProductAlternativesAsync(productId);
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
            Id = productDto.Id,
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

    private async Task PublishToMessageQueue(string integrationEvent, string eventData)
    {
        await _integrationEventRepository.AddAsync(
            new IntegrationEventDto()
            {
                Event = integrationEvent,
                Data = eventData
            }
        );
    }
}