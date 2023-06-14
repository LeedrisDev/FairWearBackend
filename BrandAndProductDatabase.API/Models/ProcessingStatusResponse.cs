
using System.Net;
using BrandAndProductDatabase.API.Models.Response;

namespace BrandAndProductDatabase.API.Models;

/// <summary>Class to handle the response of the Database</summary>
public class ProcessingStatusResponse<T>
{
    /// <summary>HTTP status code that will be returned on the API endpoint.</summary>
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    
    /// <summary><see cref="ErrorResponse"/> to be returned</summary>
    public ErrorResponse MessageObject { get; } = new();
    
    /// <summary>Message to be displayed if something went wrong</summary>
    public string ErrorMessage
    {
        get => MessageObject.Message;
        set => MessageObject.Message = value;
    }

    /// <summary>Object to be returned to the API endpoint.</summary>
    public T Object { get; set; } = default!;
}