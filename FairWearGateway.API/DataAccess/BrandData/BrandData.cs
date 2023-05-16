using System.Text.Json;
using FairWearGateway.API.Models;
using FairWearGateway.API.Models.ServiceResponse;
using FairWearGateway.API.Utils;
using FairWearGateway.API.Utils.HttpClientWrapper;

namespace FairWearGateway.API.DataAccess.BrandData;

public class BrandData : IBrandData
{
    private readonly IHttpClientWrapper _httpClientWrapper;

    public BrandData(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }
    
    public async Task<ProcessingStatusResponse<IEnumerable<BrandServiceResponse>>> GetAllBrandsAsync()
    {
        var processingStatusResponse = new ProcessingStatusResponse<IEnumerable<BrandServiceResponse>>();
        
        var response = await _httpClientWrapper.GetAsync(AppConstants.BRAND_AND_PRODUCT_API_URL);

        if (!response.IsSuccessStatusCode)
        {
            processingStatusResponse.Status = response.StatusCode;
            processingStatusResponse.ErrorMessage = response.ReasonPhrase ?? "Error while getting brands";
            return processingStatusResponse;
        }

        try
        {
            var brands = await DeserializeResponse<IEnumerable<BrandServiceResponse>>(response);
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

    public async Task<ProcessingStatusResponse<BrandServiceResponse>> GetBrandByIdAsync(int brandId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<BrandServiceResponse>();
        
        var response = await _httpClientWrapper.GetAsync($"{AppConstants.BRAND_AND_PRODUCT_API_URL}/{brandId}");
        
        if (!response.IsSuccessStatusCode)
        {
            processingStatusResponse.Status = response.StatusCode;
            processingStatusResponse.ErrorMessage = response.ReasonPhrase ?? "Error while getting brand";
            return processingStatusResponse;
        }
        
        try
        {
            var brand = await DeserializeResponse<BrandServiceResponse>(response);
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