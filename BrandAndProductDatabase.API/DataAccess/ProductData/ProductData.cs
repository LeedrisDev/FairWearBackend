using System.Net;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Utils;
using BrandAndProductDatabase.API.Utils.HttpClientWrapper;
using Newtonsoft.Json;

namespace BrandAndProductDatabase.API.DataAccess.ProductData;

/// <summary>Class to contact appropriate microservice for product data.</summary>
public class ProductData : IProductData
{
    private readonly IHttpClientWrapper _httpClientWrapper;

    /// <summary>Constructor.</summary>
    public ProductData(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> GetProductByBarcode(string barcode)
    {
        var processingStatusResponse = new ProcessingStatusResponse<ProductDto>();
        
        var response = await _httpClientWrapper.GetAsync($"{AppConstants.ProductDataRetrieverUrl}/{barcode}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Product with barcode {barcode} not found.";
            return processingStatusResponse;
        }

        if (!response.IsSuccessStatusCode)
        {
            processingStatusResponse.Status = HttpStatusCode.ServiceUnavailable;
            processingStatusResponse.ErrorMessage = $"Barcode service is unavailable.";
            return processingStatusResponse;
        }

        try
        {
            var productDto = DeserializeProductDto(response);
            processingStatusResponse.Object = productDto;
            return processingStatusResponse;
        }
        catch (ApplicationException e)
        {
            processingStatusResponse.Status = HttpStatusCode.InternalServerError;
            processingStatusResponse.ErrorMessage = e.Message;
            return processingStatusResponse;
        }
    }
    
    private static ProductDto DeserializeProductDto(HttpResponseMessage response)
    {
        var json =  response.Content.ReadAsStringAsync().Result;
        var deserializedObject = JsonConvert.DeserializeObject<ProductDto>(json);
        if (deserializedObject == null)
            throw new ApplicationException("Could not deserialize product.");
        
        return deserializedObject;
    }
}