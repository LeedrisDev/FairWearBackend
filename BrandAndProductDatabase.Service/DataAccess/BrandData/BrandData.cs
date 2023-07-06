using System.Net;
using System.Text;
using BrandAndProductDatabase.Service.Models;
using BrandAndProductDatabase.Service.Models.Dto;
using BrandAndProductDatabase.Service.Protos;
using BrandAndProductDatabase.Service.Utils;
using BrandAndProductDatabase.Service.Utils.HttpClientWrapper;
using Newtonsoft.Json;

namespace BrandAndProductDatabase.Service.DataAccess.BrandData;

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
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClientWrapper.PostAsync(AppConstants.GoodOnYouScrapperUrl, content);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var brandDto = JsonConvert.DeserializeObject<BrandDto>(responseString);
            if (brandDto == null)
            {
                processingStatusResponse.Status = HttpStatusCode.InternalServerError;
                processingStatusResponse.ErrorMessage = "Error deserializing brand.";
                return processingStatusResponse;
            }

            brandDto.Name = name;
            processingStatusResponse.Object = brandDto;
            processingStatusResponse.Status = response.StatusCode;
            return processingStatusResponse;
        }

        processingStatusResponse.Status = response.StatusCode;
        processingStatusResponse.ErrorMessage = response.ReasonPhrase ?? "Error getting brand by name.";
        return processingStatusResponse;
    }
}