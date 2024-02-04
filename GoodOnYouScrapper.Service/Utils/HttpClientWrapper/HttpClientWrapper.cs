namespace GoodOnYouScrapper.Service.Utils.HttpClientWrapper;

/// <summary>Wrapper for the <see cref="HttpClient"/> class</summary>
public class HttpClientWrapper: IHttpClientWrapper
{
    private readonly HttpClient _httpClient;
    
    /// <summary>Constructor</summary>
    /// <param name="httpClient"> <see cref="HttpClient"/> to use for the requests</param>
    public HttpClientWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    /// <inheritdoc />
    public Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        return _httpClient.GetAsync(requestUri);
    }
}