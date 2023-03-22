using System.Net;
using GoodOnYouScrapperAPI.DataAccess.BrandData;
using GoodOnYouScrapperAPI.Models;
using GoodOnYouScrapperAPI.Utils.AppConstants;
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
        catch (HttpRequestException e)
        {
            _processingStatusResponse.Status = e.StatusCode?? HttpStatusCode.InternalServerError;
            _processingStatusResponse.ErrorMessage = e.Message;
            return _processingStatusResponse;
        }
        
        // TODO: Implement the rest of the logic
        _processingStatusResponse.Status = HttpStatusCode.OK;
        
        

        var brandModel = new BrandModel();
        brandModel.name = brandName;
        
        brandModel.environmentRating = GetRating(htmlDocument, AppConstants.XPathEnvironmentRating);
        brandModel.peopleRating = GetRating(htmlDocument, AppConstants.XPathPeopleRating);
        brandModel.animalRating = GetRating(htmlDocument, AppConstants.XPathAnimalRating);
        
        _processingStatusResponse.Object.environmentRatingDescription = GetEnvironmentRatingDescription(htmlDocument);
        
        return _processingStatusResponse;
    }

    private static int GetRating(HtmlDocument doc, string xpath)
    {
        var res = doc
            .DocumentNode
            .SelectNodes(xpath)
            .First()
            .InnerHtml;

        if (res != null)
        {
            return res[0] - '0';
        }
        return -1;
    }

    private static string GetEnvironmentRatingDescription(HtmlDocument doc)
    {
        var res = doc
            .DocumentNode
            .SelectNodes("//*[@id='rating-summary-text']")
            .First()
            .ChildNodes
            .Count;
        
        Console.WriteLine(res);


        return "";
    }
    
}