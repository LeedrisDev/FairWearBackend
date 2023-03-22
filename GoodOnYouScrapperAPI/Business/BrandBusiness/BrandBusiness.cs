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
        var environmentRating = getEnvironmentRating(htmlDocument);
        var peopleRating = getPeopleRating(htmlDocument);
        var animalRating = getAnimalRating(htmlDocument);
        
        _processingStatusResponse.Status = HttpStatusCode.OK;
        _processingStatusResponse.Object = new BrandModel(environmentRating, peopleRating, animalRating);

        return _processingStatusResponse;
    }

    public int getEnvironmentRating(HtmlDocument doc)
    {
        var res = doc
            .DocumentNode
            .SelectNodes("//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[1]/div/div[2]/div/span")
            .First()
            .InnerHtml;

        if (res != null)
        {
            return res[0] - '0';
        }
        return -1;
    }
    
    public int getPeopleRating(HtmlDocument doc)
    {
        var res = doc
            .DocumentNode
            .SelectNodes("//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[2]/div/div[2]/div/span")
            .First()
            .InnerHtml;

        if (res != null)
        {
            return res[0] - '0';
        }
        return -1;
    }


    public int getAnimalRating(HtmlDocument doc)
    {
        var res = doc
            .DocumentNode
            .SelectNodes("//*[@id='__next']/div/div[4]/div/div[1]/div[2]/div[2]/div/div[1]/div[3]/div/div[2]/div/span")
            .First()
            .InnerHtml;

        if (res != null)
        {
            return res[0] - '0';
        }
        return -1;
    }
    
}