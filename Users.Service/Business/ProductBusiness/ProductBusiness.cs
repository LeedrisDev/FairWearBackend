using Users.Service.DataAccess.IRepositories;
using Users.Service.Models.Dto;

namespace Users.Service.Business.ProductBusiness;

/// <summary>Business logic for products database actions </summary>
public class ProductBusiness : IProductBusiness
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Constructor for ProductBusiness.
    /// </summary>
    /// <param name="productRepository"></param>
    public ProductBusiness(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <inheritdoc/>
    public async Task CreateProductAsync(ProductDto productDto)
    {
        await _productRepository.AddAsync(productDto);
    }

    /// <inheritdoc/>
    public async Task UpdateProductAsync(ProductDto productDto)
    {
        await _productRepository.UpdateAsync(productDto);
    }

    /// <inheritdoc/>
    public async Task DeleteProductAsync(long id)
    {
        await _productRepository.DeleteAsync(id);
    }
}