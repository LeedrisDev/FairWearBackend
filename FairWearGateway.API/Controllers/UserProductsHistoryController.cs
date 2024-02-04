using System.ComponentModel.DataAnnotations;
using System.Net;
using FairWearGateway.API.Business.UserProductHistoryBusiness;
using FairWearGateway.API.Models.Request;
using FairWearGateway.API.Models.Response;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Users.Service;

namespace FairWearGateway.API.Controllers;

/// <summary>
/// Controller for managing user product history.
/// </summary>
[ApiController]
[Route("/api/history/")]
[Produces("application/json")]
public class UserProductsHistoryController : ControllerBase
{
    private readonly IUserProductHistoryBusiness _userProductHistoryBusiness;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserProductsHistoryController"/> class.
    /// </summary>
    /// <param name="userProductHistoryBusiness">The user product history business service.</param>
    public UserProductsHistoryController(IUserProductHistoryBusiness userProductHistoryBusiness)
    {
        _userProductHistoryBusiness = userProductHistoryBusiness;
    }

    /// <summary>
    /// Retrieves user product history by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The user product history response.</returns>
    [HttpGet("{userId:long}")]
    [ProducesResponseType(typeof(GetUserProductHistoryResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
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

    /// <summary>
    /// Creates user product history.
    /// </summary>
    /// <param name="request">The request to create user product history.</param>
    /// <returns>The created user product history.</returns>
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

    /// <summary>
    /// Updates user product history.
    /// </summary>
    /// <param name="request">The user product history details to update.</param>
    /// <returns>The updated user product history.</returns>
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

    /// <summary>
    /// Deletes user product history by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The deleted user product history.</returns>
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