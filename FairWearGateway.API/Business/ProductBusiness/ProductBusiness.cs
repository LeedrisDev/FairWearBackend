using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.DataAccess.ProductData;
using FairWearGateway.API.Models;

namespace FairWearGateway.API.Business.ProductBusiness;

/// <summary>Class that handles the business logic for the Product model.</summary>
public class ProductBusiness : IProductBusiness
{
    private readonly IProductData _productData;

    /// <summary>Initializes a new instance of the <see cref="ProductBusiness"/> class.</summary>
    public ProductBusiness(IProductData productData)
    {
        _productData = productData;
    }

    /// <inheritdoc/>
    public ProcessingStatusResponse<ProductResponse> GetProductById(int productId)
    {
        return _productData.GetProductById(productId);
    }

    /// <inheritdoc />
    public ProcessingStatusResponse<ProductInformationResponse> GetProductByUpc(string upc)
    {
        return _productData.GetProductByUpc(upc);
    }

    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<IEnumerable<ProductResponse>>> GetAllProducts(
        Dictionary<string, string> filters)
    {
        return await _productData.GetAllProducts(filters);
    }
}