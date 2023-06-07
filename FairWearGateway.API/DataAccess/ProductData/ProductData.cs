using System.Text;
using BrandAndProductDatabase.API.Models.Response;
using FairWearGateway.API.Models;
using FairWearGateway.API.Utils;
using FairWearGateway.API.Utils.HttpClientWrapper;
using Newtonsoft.Json;

namespace FairWearGateway.API.DataAccess.ProductData;

/// <summary>Class that implements the <see cref="IProductData"/> interface.</summary>
public class ProductData : IProductData
{
    private readonly IHttpClientWrapper _httpClientWrapper;

    /// <summary>Constructor</summary>
    public ProductData(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }
    
    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<IEnumerable<ProductResponse>>> GetAllProductsAsync()
    {
        var processingStatusResponse = new ProcessingStatusResponse<IEnumerable<ProductResponse>>();
        
        var response = await _httpClientWrapper.GetAsync($"{AppConstants.BrandAndProductApiUrl}/products");

        if (!response.IsSuccessStatusCode)
        {
            processingStatusResponse.Status = response.StatusCode;
            processingStatusResponse.ErrorMessage = response.ReasonPhrase ?? "Error while getting products";
            return processingStatusResponse;
        }

        try
        {
            var products = await DeserializeResponse<IEnumerable<ProductResponse>>(response);
            processingStatusResponse.Object = products;
            return processingStatusResponse;
        }
        catch (ApplicationException e)
        {
            processingStatusResponse.Status = System.Net.HttpStatusCode.InternalServerError;
            processingStatusResponse.ErrorMessage = e.Message;
            return processingStatusResponse;
        }
    }

    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<ProductResponse>> GetProductByIdAsync(int productId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<ProductResponse>();
        
        var response = await _httpClientWrapper.GetAsync($"{AppConstants.BrandAndProductApiUrl}/product/{productId}");
        
        if (!response.IsSuccessStatusCode)
        {
            processingStatusResponse.Status = response.StatusCode;
            processingStatusResponse.ErrorMessage = response.ReasonPhrase ?? "Error while getting product";
            return processingStatusResponse;
        }
        
        try
        {
            var product = await DeserializeResponse<ProductResponse>(response);
            processingStatusResponse.Object = product;
            return processingStatusResponse;
        }
        catch (ApplicationException e)
        {
            processingStatusResponse.Status = System.Net.HttpStatusCode.InternalServerError;
            processingStatusResponse.ErrorMessage = e.Message;
            return processingStatusResponse;
        }
    }
    

    private static async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        var deserializedObject = JsonConvert.DeserializeObject<T>(responseString);

        if (deserializedObject == null)
            throw new ApplicationException("Cannot deserialize response");

        return deserializedObject;
    }
}