using System.ComponentModel.DataAnnotations;
using System.Net;
using FairWearGateway.API.Business.UserBusiness;
using FairWearGateway.API.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Users.Service;

namespace FairWearGateway.API.Controllers;

/// <summary>
/// Controller that handles the requests for the User model.
/// </summary>
[ApiController]
[Route("/api/")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserBusiness _userBusiness;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userBusiness">The user business service.</param>
    public UsersController(IUserBusiness userBusiness)
    {
        _userBusiness = userBusiness;
    }

    /// <summary>
    /// Gets a user by its Firebase ID.
    /// </summary>
    /// <param name="firebaseId">The Firebase ID of the user.</param>
    /// <returns>An action result containing the user or an error response.</returns>
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

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user details to create.</param>
    /// <returns>An action result containing the created user or an error response.</returns>
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

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="request">The user details to update.</param>
    /// <returns>An action result containing the updated user or an error response.</returns>
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

    /// <summary>
    /// Deletes a user by its Firebase ID.
    /// </summary>
    /// <param name="firebaseId">The Firebase ID of the user.</param>
    /// <returns>An action result indicating the result of the deletion or an error response.</returns>
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