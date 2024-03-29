﻿using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;

namespace BrandAndProduct.Service.Business.ProductBusiness;

/// <summary>
/// Interface for product business
/// </summary>
public interface IProductBusiness
{
    /// <summary>Gets all the Products.</summary>
    /// <returns>A list of <see cref="Models.Dto.ProductDto"/>.</returns>
    Task<ProcessingStatusResponse<IEnumerable<ProductDto>>> GetAllProductsAsync(Dictionary<string, string> filters);

    /// <summary>Gets a Product by Id.</summary>
    /// <param name="id">The Id of the Product.</param>
    /// <returns>A <see cref="ProductDto"/>.</returns>
    Task<ProcessingStatusResponse<ProductDto>> GetProductByIdAsync(int id);


    /// <summary>
    /// Returns a product information by its upc code
    /// </summary>
    /// <param name="upcCode">barcode of the product</param>
    /// <returns>A <see cref="ProductInformationDto"/>.</returns>
    Task<ProcessingStatusResponse<ProductInformationDto>> GetProductByUpcAsync(string upcCode);

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
    Task<ProcessingStatusResponse<ProductDto>> DeleteProductAsync(int id);
    
    /// <summary>
    /// Get recommended products for a product.
    /// </summary>
    /// <param name="productId">Product id to get alternatives products for.</param>
    /// <returns></returns>
    Task<ProcessingStatusResponse<IEnumerable<ProductDto>>> GetProductAlternativesAsync(int productId);
}