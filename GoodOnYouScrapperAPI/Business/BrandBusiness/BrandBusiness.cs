using System.Net;
using System.Runtime.InteropServices;
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
        
        // TODO: Implement the rest of the logic
        
        var brandModel = new BrandModel();
        brandModel.Name = brandName;
        brandModel.Country = GetBrandCountry(htmlDocument);
        
        brandModel.EnvironmentRating = GetRating(htmlDocument, AppConstants.XPathEnvironmentRating);
        brandModel.PeopleRating = GetRating(htmlDocument, AppConstants.XPathPeopleRating);
        brandModel.AnimalRating = GetRating(htmlDocument, AppConstants.XPathAnimalRating);
        
        brandModel.RatingDescription = GetRatingDescription(htmlDocument);
        brandModel.Categories = GetBrandCategories(htmlDocument);
        brandModel.Ranges = GetBrandRanges(htmlDocument);


        _processingStatusResponse.Status = HttpStatusCode.OK;
        _processingStatusResponse.Object = brandModel;
        
        return _processingStatusResponse;
    }
    
    
    /// <summary>
    /// Retrieves the rating for a specific category
    /// </summary>
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

    /// <summary>
    /// Retrieves the html of the overall rating description
    /// </summary>
    /// <param name="doc">Document to retrieve the information from</param>
    /// <returns></returns>
    private static string GetRatingDescription(HtmlDocument doc)
    {
        var ratingSummaryNodes = doc
            .DocumentNode
            .SelectNodes(AppConstants.XPathRatingDescription)
            .First();

        return ratingSummaryNodes.InnerText;
    }
    
    /// <summary>
    /// Retrieves the location of a brand
    /// </summary>
    /// <param name="doc">Document to retrieve the information from</param>
    /// <returns></returns>
    private static string GetBrandCountry(HtmlDocument doc)
    {
        var country = doc
            .DocumentNode
            .SelectNodes(AppConstants.XPathBrandCountry)
            .First()
            .InnerText;
        
        if (country.StartsWith("location: "))
            return country.Substring("location: ".Length);
        
        return "";
    }
    
    private static string[] GetBrandCategories(HtmlDocument doc)
    {
        var categories = doc
            .DocumentNode
            .SelectNodes(AppConstants.XPathBrandCategories)
            .First()
            .ChildNodes;
        
        var listCategories = new List<String>();

        foreach (var category in categories)
        {
            listCategories.Add(category.InnerText);
        }

        return listCategories.ToArray();
    }
    
    
    private static string[] GetBrandRanges(HtmlDocument doc)
    {
        var ranges = doc
            .DocumentNode
            .SelectNodes(AppConstants.XPathBrandRanges)
            .First()
            .ChildNodes;

        ranges.RemoveAt(0);
        var listRanges = new List<String>();
        
        foreach (var range in ranges)
        {
            string rangeText = range.InnerText;

            if (rangeText.EndsWith(','))
                rangeText = rangeText.Remove(rangeText.Length - 1);
            listRanges.Add(rangeText);
        }

        return listRanges.ToArray();
    }
    
    
}