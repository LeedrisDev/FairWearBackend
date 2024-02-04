using Users.Service.Models.Dto;

namespace Users.Service.Business.ProductBusiness;

/// <summary>
/// Interface for ProductBusiness.
/// </summary>
public interface IProductBusiness
{
    /// <summary>Creates a Product.</summary>
    /// <param name="productDto">The Product to create.</param>
    /// <returns>A <see cref="ProductDto"/>.</returns>
    Task CreateProductAsync(ProductDto productDto);

    /// <summary>Updates a Product.</summary>
    /// <param name="productDto">The Product to update.</param>
    /// <returns>A <see cref="ProductDto"/>.</returns>
    Task UpdateProductAsync(ProductDto productDto);

    /// <summary>Deletes a Product.</summary>
    /// <param name="id">The Id of the Product to delete.</param>
    /// <returns>A <see cref="Task"/>.</returns>
    Task DeleteProductAsync(long id);
}