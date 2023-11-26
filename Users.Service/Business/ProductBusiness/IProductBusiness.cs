using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.ProductBusiness
{
    /// <summary>
    /// Interface for ProductBusiness.
    /// </summary>
    public interface IProductBusiness
    {
        /// <summary>Gets all the Products.</summary>
        /// <returns>A list of <see cref="ProductDto"/>.</returns>
        Task<ProcessingStatusResponse<IEnumerable<ProductDto>>> GetAllProductsAsync(Dictionary<string, string> filters);

        /// <summary>Gets a Product by Id.</summary>
        /// <param name="id">The Id of the Product.</param>
        /// <returns>A <see cref="ProductDto"/>.</returns>
        Task<ProcessingStatusResponse<ProductDto>> GetProductByIdAsync(long id);

        /// <summary>Creates a Product.</summary>
        /// <param name="productDto">The Product to create.</param>
        /// <returns>A <see cref="ProductDto"/>.</returns>
        Task<ProcessingStatusResponse<ProductDto>> CreateProductAsync(ProductDto productDto);

        /// <summary>Updates a Product.</summary>
        /// <param name="productDto">The Product to update.</param>
        /// <returns>A <see cref="ProductDto"/>.</returns>
        Task<ProcessingStatusResponse<ProductDto>> UpdateProductAsync(ProductDto productDto);

        /// <summary>Deletes a Product.</summary>
        /// <param name="id">The Id of the Product to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task<ProcessingStatusResponse<ProductDto>> DeleteProductAsync(long id);
    }
}