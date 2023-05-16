using System.Net;
using System.Text;
using System.Text.Json;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Models.Request;
using BrandAndProductDatabase.API.Utils;
using BrandAndProductDatabase.API.Utils.HttpClientWrapper;

namespace BrandAndProductDatabase.API.DataAccess.BrandData;

/// <summary>Class to contact appropriate microservice for brand data.</summary>
public class BrandData : IBrandData
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    
    /// <summary>Initializes a new instance of the <see cref="BrandData"/> class.</summary>
    public BrandData(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> GetBrandByNameAsync(string name)
    {
        var processingStatusResponse = new ProcessingStatusResponse<BrandDto>();
        
        var data = new BrandRequest { Name = name };
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClientWrapper.PostAsync(AppConstants.GOOD_ON_YOU_SCRAPPER_URL, content);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var brandDto = JsonSerializer.Deserialize<BrandDto>(responseString);
            if (brandDto == null)
            {
                processingStatusResponse.Status = HttpStatusCode.InternalServerError;
                processingStatusResponse.ErrorMessage = "Error deserializing brand.";
                return processingStatusResponse;
            }
            
            processingStatusResponse.Object = brandDto;
            processingStatusResponse.Status = response.StatusCode;
            return processingStatusResponse;
        }

        processingStatusResponse.Status = response.StatusCode;
        processingStatusResponse.ErrorMessage = response.ReasonPhrase ?? "Error getting brand by name.";
        return processingStatusResponse;
    }
}