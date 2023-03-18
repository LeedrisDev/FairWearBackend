using System.Net;

namespace GoodOnYouScrapperAPI.Models;

/// <summary>Class to handle the response of the API</summary>
public class ProcessingStatusResponse<T>
{
    /// <summary>HTTP status code</summary>
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    
    /// <summary>Message to be displayed if something went wrong</summary>
    public ErrorResponse MessageObject { get; set; } = new();

    /// <summary>Object to be returned</summary>
    public T Object { get; set; } = default!;
}