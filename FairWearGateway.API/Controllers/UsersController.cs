using System.ComponentModel.DataAnnotations;
using System.Net;
using FairWearGateway.API.Business.UserBusiness;
using FairWearGateway.API.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Users.Service;

namespace FairWearGateway.API.Controllers;

/// <summary>Controller that handles the requests for the User model.</summary>
[ApiController]
[Route("/api/")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserBusiness _userBusiness;

    public UsersController(IUserBusiness userBusiness)
    {
        _userBusiness = userBusiness;
    }

    /// <summary>Gets a user by its firebase id.</summary>
    [HttpGet("user/{firebaseId}")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetUserByFirebaseId([Required] string firebaseId)
    {
        var processingStatusResponse = _userBusiness.GetUserByFirebaseId(firebaseId);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    /// <summary>Create a new user.</summary>
    [HttpPost("user")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult CreateUser(User request)
    {
        var processingStatusResponse = _userBusiness.CreateUser(request);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    /// <summary>Update a new user.</summary>
    [HttpPut("user")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult UpdateUser(User request)
    {
        var processingStatusResponse = _userBusiness.UpdateUser(request);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    /// <summary>Delete a user by its firebase id.</summary>
    [HttpDelete("user/{firebaseId}")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult DeleteUserByFirebaseId([Required] string firebaseId)
    {
        var processingStatusResponse = _userBusiness.DeleteUserByFirebaseId(firebaseId);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }
}