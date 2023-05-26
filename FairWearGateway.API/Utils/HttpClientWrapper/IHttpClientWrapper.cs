namespace FairWearGateway.API.Utils.HttpClientWrapper;

/// <summary>Wrapper for the <see cref="HttpClient"/> class</summary>
public interface IHttpClientWrapper
{
    /// <summary>Send a GET request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <throws>
    /// <para><see cref="InvalidOperationException"/> – The requestUri must be an absolute URI or BaseAddress must be set.</para>
    /// <para><see cref="HttpRequestException"/> – The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</para>
    /// <para><see cref="TaskCanceledException"/> – .NET Core and .NET 5 and later only: The request failed due to timeout.</para>
    /// <para><see cref="UriFormatException"/> – The provided request URI is not valid relative or absolute URI.</para>
    /// </throws>
    Task<HttpResponseMessage> GetAsync(string requestUri);
    
    /// <summary>Send a POST request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <throws>
    /// <para><see cref="InvalidOperationException"/> – The requestUri must be an absolute URI or BaseAddress must be set.</para>
    /// <para><see cref="HttpRequestException"/> – The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</para>
    /// <para><see cref="TaskCanceledException"/> – .NET Core and .NET 5 and later only: The request failed due to timeout.</para>
    /// <para><see cref="UriFormatException"/> – The provided request URI is not valid relative or absolute URI.</para>
    /// </throws>
    Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
}