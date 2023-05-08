using BrandAndProductDatabase.API.Models;

namespace BrandAndProductDatabase.API.Business.ProductBusiness;

public interface IProductBusiness
{
    /// <summary>Gets all the Products.</summary>
    /// <returns>A list of <see cref="Models.Dto.ProductDto"/>.</returns>
    Task<ProcessingStatusResponse<IEnumerable<Models.Dto.ProductDto>>> GetAllProductsAsync();

    /// <summary>Gets a Product by Id.</summary>
    /// <param name="id">The Id of the Product.</param>
    /// <returns>A <see cref="Models.Dto.ProductDto"/>.</returns>
    Task<ProcessingStatusResponse<Models.Dto.ProductDto>> GetProductByIdAsync(int id);

    /// <summary>Creates a Product.</summary>
    /// <param name="ProductDto">The Product to create.</param>
    /// <returns>A <see cref="ProductDto"/>.</returns>
    Task<ProcessingStatusResponse<Models.Dto.ProductDto>> CreateProductAsync(Models.Dto.ProductDto ProductDto);

    /// <summary>Updates a Product.</summary>
    /// <param name="ProductDto">The Product to update.</param>
    /// <returns>A <see cref="ProductDto"/>.</returns>
    Task<ProcessingStatusResponse<Models.Dto.ProductDto>> UpdateProductAsync(Models.Dto.ProductDto ProductDto);

    /// <summary>Deletes a Product.</summary>
    /// <param name="id">The Id of the Product to delete.</param>
    /// <returns>A <see cref="Task"/>.</returns>
    Task<ProcessingStatusResponse<Models.Dto.ProductDto>> DeleteProductAsync(int id);
}