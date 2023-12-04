using System.ComponentModel.DataAnnotations;
using System.Net;
using FairWearGateway.API.Business.UserProductHistoryBusiness;
using FairWearGateway.API.Models.Request;
using FairWearGateway.API.Models.Response;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Users.Service;

namespace FairWearGateway.API.Controllers;

[ApiController]
[Route("/api/history/")]
[Produces("application/json")]
public class UserProductsHistoryController : ControllerBase
{
    private readonly IUserProductHistoryBusiness _userProductHistoryBusiness;

    public UserProductsHistoryController(IUserProductHistoryBusiness userProductHistoryBusiness)
    {
        _userProductHistoryBusiness = userProductHistoryBusiness;
    }

    [HttpGet("{userId:long}")]
    [ProducesResponseType(typeof(UserProductHistory), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(UserProductHistory), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetUserProductHistoryByUserId([Required] long userId)
    {
        var processingStatusResponse = _userProductHistoryBusiness.GetUserProductHistoryByUserId(userId);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    [HttpPost()]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult CreateUserProductHistory(CreateUserProductHistoryRequest request)
    {
        var userProductHistory = new UserProductHistory
        {
            UserId = request.UserId,
            ProductId = request.ProductId,
            Timestamp = Timestamp.FromDateTime(DateTime.UtcNow.ToUniversalTime())
        };
        
        var processingStatusResponse = _userProductHistoryBusiness.CreateUserProductHistory(userProductHistory);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    [HttpPut()]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult UpdateUserProductHistory(UserProductHistory request)
    {
        var processingStatusResponse = _userProductHistoryBusiness.UpdateUserProductHistory(request);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }


    [HttpDelete("{userId:long}")]
    [ProducesResponseType(typeof(UserProductHistory), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(UserProductHistory), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult DeleteUserProductHistory([Required] long userId)
    {
        var processingStatusResponse = _userProductHistoryBusiness.DeleteUserProductHistory(userId);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }
}