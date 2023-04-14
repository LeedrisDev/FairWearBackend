using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
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
        
        // TODO: Implement the rest of the logic
        
        var brandModel = new BrandModel
        {
            Name = brandName,
            Country = GetBrandCountry(htmlDocument),
            EnvironmentRating = GetRating(htmlDocument, AppConstants.XPathEnvironmentRating),
            PeopleRating = GetRating(htmlDocument, AppConstants.XPathPeopleRating),
            AnimalRating = GetRating(htmlDocument, AppConstants.XPathAnimalRating),
            RatingDescription = GetRatingDescription(htmlDocument),
            Categories = GetBrandCategories(htmlDocument),
            Ranges = GetBrandRanges(htmlDocument),
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
            .First()
            .ChildNodes;

        StringBuilder sb = new StringBuilder();
        
        foreach (var node in ratingSummaryNodes)
        {
            if (node.Name == "ul")
            {
                foreach (var li in node.ChildNodes)
                {
                    sb.Append("\t" + "- "+ li.InnerText + "\n");
                }
            }
            else
            {
                sb.Append(node.InnerText + "\n");
            }

            if (node.NextSibling is { Name: "p" })
                sb.Append('\n');
        }
        
        return sb.ToString();
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
        
        return country.StartsWith("location: ") ? country.Substring("location: ".Length) : "";
    }
    
    private static string[] GetBrandCategories(HtmlDocument doc)
    {
        var sideBar = doc
            .DocumentNode
            .SelectNodes(AppConstants.XPathSideBar)
            .First()
            .ChildNodes;
        
        var listCategories = new List<String>();
        var divList = sideBar.Where(row => row.Name == "div").ToList();


        foreach (var row in divList)
        {
            if (row.FirstChild.InnerText == "CATEGORIES")
            {
                row.ChildNodes.RemoveAt(0);
                foreach (var node in row.FirstChild.ChildNodes)
                {
                    var str = HttpUtility.HtmlDecode(node.InnerText);
                    listCategories.Add(str);
                }
                return listCategories.ToArray();

            }
        }
        return listCategories.ToArray();
    }
    
    
    private static string[] GetBrandRanges(HtmlDocument doc)
    {
        var sideBar = doc
            .DocumentNode
            .SelectNodes(AppConstants.XPathSideBar)
            .First()
            .ChildNodes;
        
        var divList = sideBar.Where(row => row.Name == "div").ToList();
        
        var listRanges = new List<String>();
        
        foreach (var row in divList)
        {
            if (row.FirstChild.InnerText == "RANGE")
            {
                row.ChildNodes.RemoveAt(0);
                foreach (var node in row.ChildNodes)
                {
                    var str = HttpUtility.HtmlDecode(node.InnerText);
                    str = Regex.Replace(str, @"[^\w]", string.Empty);
                    listRanges.Add(str);
                }

                return listRanges.ToArray();
            }
        }

        return listRanges.ToArray();
    }


}