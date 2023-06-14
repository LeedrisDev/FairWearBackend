using System.Text;
using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Request;
using FairWearGateway.API.Models.Response;
using FairWearGateway.API.Utils;
using FairWearGateway.API.Utils.HttpClientWrapper;
using Newtonsoft.Json;

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
    public async Task<ProcessingStatusResponse<BrandResponse>> GetBrandByIdAsync(int brandId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<BrandResponse>();

        var response = await _httpClientWrapper.GetAsync($"{AppConstants.BrandAndProductApiUrl}/brand/{brandId}");

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

    /// <inheritdoc />
    public async Task<ProcessingStatusResponse<BrandResponse>> GetBrandByNameAsync(string name)
    {
        var request = new BrandRequest { Name = name };
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var processingStatusResponse = new ProcessingStatusResponse<BrandResponse>();

        var response = await _httpClientWrapper.PostAsync($"{AppConstants.BrandAndProductApiUrl}/brand/name", content);

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
        var deserializedObject = JsonConvert.DeserializeObject<T>(responseString);

        if (deserializedObject == null)
            throw new ApplicationException("Cannot deserialize response");

        return deserializedObject;
    }
}