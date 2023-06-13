using FairWearGateway.API.DataAccess.ProductData;
using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Response;

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
    public async Task<ProcessingStatusResponse<ProductResponse>> GetProductByIdAsync(int productId)
    {
        return await _productData.GetProductByIdAsync(productId);
    }

    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<ProductInformationResponse>> GetProductByUpcAsync(string upc)
    {
        return await _productData.GetProductByUpcAsync(upc);
    }
}