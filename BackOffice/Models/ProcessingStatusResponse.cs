using System.Net;

namespace BackOffice.Models;

/// <summary>
/// Represents a response with processing status and an associated entity of type <typeparamref name="TModel"/>.
/// </summary>
/// <typeparam name="TModel">The type of entity associated with the response.</typeparam>
public class ProcessingStatusResponse<TModel>
{
    /// <summary>Gets or sets the HTTP status code of the response. Default is <see cref="HttpStatusCode.OK"/>.</summary>
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    /// <summary>Gets or sets a message associated with the response. Default is an empty string.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>Gets or sets the entity associated with the response.</summary>
    public TModel Entity { get; set; } = default!;
}
