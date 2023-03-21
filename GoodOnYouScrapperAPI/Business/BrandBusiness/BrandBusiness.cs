using System.Net;
using GoodOnYouScrapperAPI.DataAccess.BrandData;
using GoodOnYouScrapperAPI.Models;
using HtmlAgilityPack;

namespace GoodOnYouScrapperAPI.Business.BrandBusiness;

public class BrandBusiness: IBrandBusiness
{
    private ProcessingStatusResponse<BrandModel> _processingStatusResponse;
    private readonly IBrandData _brandData;
    
    public BrandBusiness(IBrandData brandData)
    {
        _processingStatusResponse = new ProcessingStatusResponse<BrandModel>();
        _brandData = brandData;
    }
    
    public async Task<ProcessingStatusResponse<BrandModel>> GetBrandInformation(string brandName)
    {
        HtmlDocument htmlDocument;

        try
        {
            htmlDocument = await _brandData.GetBrandPageHtml(brandName);
        }
        catch (Exception e)
        {
            _processingStatusResponse.Status = HttpStatusCode.NotFound;
            _processingStatusResponse.ErrorMessage = e.Message;
            return _processingStatusResponse;
        }
        
        // TODO: Implement the rest of the logic

        throw new NotImplementedException();
    }
}