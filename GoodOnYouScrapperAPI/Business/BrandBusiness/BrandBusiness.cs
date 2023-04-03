using System.Net;
using GoodOnYouScrapperAPI.DataAccess.BrandData;
using GoodOnYouScrapperAPI.Models;
using GoodOnYouScrapperAPI.Utils.AppConstants;
using HtmlAgilityPack;

namespace GoodOnYouScrapperAPI.Business.BrandBusiness;

public class BrandBusiness: IBrandBusiness
{
    private readonly ProcessingStatusResponse<BrandModel> _processingStatusResponse;
    private readonly IBrandData _brandData;

    public BrandBusiness(IBrandData brandData)
    {
        _processingStatusResponse = new ProcessingStatusResponse<BrandModel>();
        _brandData = brandData;
    }
    
    
    /// <summary>Retrieves information for a brand on the GoodOnYou website</summary>
    /// <param name="brandName">Name of the brand to retrieve information for</param>
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

        var brandModel = new BrandModel
        {
            Name = brandName,
            EnvironmentRating = GetRating(htmlDocument, AppConstants.XPathEnvironmentRating),
            PeopleRating = GetRating(htmlDocument, AppConstants.XPathPeopleRating),
            AnimalRating = GetRating(htmlDocument, AppConstants.XPathAnimalRating),
            RatingDescription = GetRatingDescription(htmlDocument)
        };

        _processingStatusResponse.Status = HttpStatusCode.OK;
        _processingStatusResponse.Object = brandModel;
        
        return _processingStatusResponse;
    }
    
    
    /// <summary>Retrieves the rating for a specific category</summary>
    /// <param name="doc">Document to retrieve the information from</param>
    /// <param name="xpath">Path of the content in the source document</param>
    /// <returns></returns>
    private static int GetRating(HtmlDocument doc, string xpath)
    {
        var rating = doc
            .DocumentNode
            .SelectNodes(xpath)
            .First()
            .InnerHtml;

        if (rating != null)
        {
            return rating[0] - '0';
        }
        return -1;
    }

    /// <summary>Retrieves the html of the overall rating description</summary>
    /// <param name="doc">Document to retrieve the information from</param>
    /// <returns></returns>
    private static string GetRatingDescription(HtmlDocument doc)
    {
        var ratingSummaryNodes = doc
            .DocumentNode
            .SelectNodes(AppConstants.XPathRatingDescription)
            .First();

        return ratingSummaryNodes.OuterHtml;
    }
    
}