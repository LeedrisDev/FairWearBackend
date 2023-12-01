using System.ComponentModel.DataAnnotations;
using System.Net;
using FairWearGateway.API.Business.UserExperienceBusiness;
using FairWearGateway.API.Models.Request;
using FairWearGateway.API.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Users.Service;

namespace FairWearGateway.API.Controllers;

[ApiController]
[Route("/api/")]
[Produces("application/json")]
public class UserExperiencesController : ControllerBase
{
    private readonly IUserExperienceBusiness _userExperienceBusiness;

    public UserExperiencesController(IUserExperienceBusiness userExperienceBusiness)
    {
        _userExperienceBusiness = userExperienceBusiness;
    }

    [HttpGet("userExperience/{userId:long}")]
    [ProducesResponseType(typeof(UserExperience), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(UserExperience), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetUserExperienceByUserId([Required] long userId)
    {
        var processingStatusResponse = _userExperienceBusiness.GetUserExperienceByUserId(userId);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    [HttpPost("userExperience")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult CreateUserExperience(UserExperienceCreateRequest request)
    {
        var userExperience = new UserExperience()
        {
            UserId = request.UserId,
            Score = request.Score ?? 0,
            Level = request.Level ?? 0,
        };

        if (request.Todos != null)
        {
            userExperience.Todos.AddRange(request.Todos);
        }

        var processingStatusResponse = _userExperienceBusiness.CreateUserExperience(userExperience);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    [HttpPut("userExperience")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult UpdateUserExperience(UserExperienceCreateRequest request)
    {
        var userExperience = new UserExperience()
        {
            Id = request.Id,
            UserId = request.UserId,
            Score = request.Score ?? 0,
            Level = request.Level ?? 0,
        };

        if (request.Todos != null)
        {
            userExperience.Todos.AddRange(request.Todos);
        }
        else
        {
            userExperience.Todos.AddRange(new List<int>() { 0, 0, 0 });
        }

        var processingStatusResponse = _userExperienceBusiness.UpdateUserExperience(userExperience);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }


    [HttpDelete("userExperience/{userId:long}")]
    [ProducesResponseType(typeof(UserExperience), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(UserExperience), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult DeleteUserExperience([Required] long userId)
    {
        var processingStatusResponse = _userExperienceBusiness.DeleteUserExperience(userId);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }
}