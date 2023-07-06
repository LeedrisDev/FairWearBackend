using System.Net;
using BrandAndProductDatabase.Service.Models;
using BrandAndProductDatabase.Service.Models.Dto;
using BrandAndProductDatabase.Service.Utils;
using BrandAndProductDatabase.Service.Utils.HttpClientWrapper;
using Newtonsoft.Json;

namespace BrandAndProductDatabase.Service.DataAccess.ProductData;

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
    public async Task<ProcessingStatusResponse<ProductRetrieverDto>> GetProductByUpc(string upc)
    {
        var processingStatusResponse = new ProcessingStatusResponse<ProductRetrieverDto>();

        var response = await _httpClientWrapper.GetAsync($"{AppConstants.ProductDataRetrieverUrl}/{upc}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Product with barcode {upc} not found.";
            return processingStatusResponse;
        }

        if (!response.IsSuccessStatusCode)
        {
            processingStatusResponse.Status = HttpStatusCode.ServiceUnavailable;
            processingStatusResponse.ErrorMessage = "Barcode service is unavailable.";
            return processingStatusResponse;
        }

        try
        {
            var productRetrieverDto = DeserializeProductRetrieverDto(response);
            processingStatusResponse.Object = productRetrieverDto;
            return processingStatusResponse;
        }
        catch (ApplicationException e)
        {
            processingStatusResponse.Status = HttpStatusCode.InternalServerError;
            processingStatusResponse.ErrorMessage = e.Message;
            return processingStatusResponse;
        }
    }

    private static ProductRetrieverDto DeserializeProductRetrieverDto(HttpResponseMessage response)
    {
        var json = response.Content.ReadAsStringAsync().Result;
        var deserializedObject = JsonConvert.DeserializeObject<ProductRetrieverDto>(json);
        if (deserializedObject == null)
            throw new ApplicationException("Could not deserialize product.");

        return deserializedObject;
    }
}