using System.Text.Json;
using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Response;
using FairWearGateway.API.Utils;
using FairWearGateway.API.Utils.HttpClientWrapper;

namespace FairWearGateway.API.DataAccess.BrandData;


/// <summary>Class that call the appropriate microservice to get all is related to brands.</summary>
public class BrandData : IBrandData
{
    private readonly IHttpClientWrapper _httpClientWrapper;

    /// <summary>Constructor</summary>
    public BrandData(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }
    
    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<IEnumerable<BrandResponse>>> GetAllBrandsAsync()
    {
        var processingStatusResponse = new ProcessingStatusResponse<IEnumerable<BrandResponse>>();
        
        var response = await _httpClientWrapper.GetAsync(AppConstants.BRAND_AND_PRODUCT_API_URL);

        if (!response.IsSuccessStatusCode)
        {
            processingStatusResponse.Status = response.StatusCode;
            processingStatusResponse.ErrorMessage = response.ReasonPhrase ?? "Error while getting brands";
            return processingStatusResponse;
        }

        try
        {
            var brands = await DeserializeResponse<IEnumerable<BrandResponse>>(response);
            processingStatusResponse.Object = brands;
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
    public async Task<ProcessingStatusResponse<BrandResponse>> GetBrandByIdAsync(int brandId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<BrandResponse>();
        
        var response = await _httpClientWrapper.GetAsync($"{AppConstants.BRAND_AND_PRODUCT_API_URL}/{brandId}");
        
        if (!response.IsSuccessStatusCode)
        {
            processingStatusResponse.Status = response.StatusCode;
            processingStatusResponse.ErrorMessage = response.ReasonPhrase ?? "Error while getting brand";
            return processingStatusResponse;
        }
        
        try
        {
            var brand = await DeserializeResponse<BrandResponse>(response);
            processingStatusResponse.Object = brand;
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
        var deserializedObject = JsonSerializer.Deserialize<T>(responseString);

        if (deserializedObject == null)
            throw new ApplicationException("Cannot deserialize response");

        return deserializedObject;
    }
}