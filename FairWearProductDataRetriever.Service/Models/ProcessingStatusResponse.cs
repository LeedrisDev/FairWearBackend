using System.Net;

namespace FairWearProductDataRetriever.Service.Models;

/// <summary>Class to handle the response of the API</summary>
public class ProcessingStatusResponse<T>
{
    /// <summary>HTTP status code</summary>
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

    /// <summary><see cref="ErrorResponse"/> to be returned</summary>
    public ErrorResponse MessageObject { get; } = new();

    /// <summary>Message to be displayed if something went wrong</summary>
    public string ErrorMessage
    {
        get => MessageObject.Message;
        set => MessageObject.Message = value;
    }

    /// <summary>Object to be returned</summary>
    public T Object { get; set; } = default!;
}