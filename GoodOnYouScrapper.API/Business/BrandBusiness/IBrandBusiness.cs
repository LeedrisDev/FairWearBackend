using GoodOnYouScrapper.API.Models;

namespace GoodOnYouScrapper.API.Business.BrandBusiness;

/// <summary>Interface for the brand business</summary>
public interface IBrandBusiness
{
    /// <summary>Retrieves information for a brand on the GoodOnYou website</summary>
    /// <param name="brandName">Name of the brand to retrieve information for</param>
    public Task<ProcessingStatusResponse<BrandModel>> GetBrandInformation(string brandName);
}