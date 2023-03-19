using GoodOnYouScrapperAPI.Models;

namespace GoodOnYouScrapperAPI.Business.BrandBusiness;

public interface IBrandBusiness
{
    public Task<ProcessingStatusResponse<BrandModel>> GetBrandInformation(string brandName);
}