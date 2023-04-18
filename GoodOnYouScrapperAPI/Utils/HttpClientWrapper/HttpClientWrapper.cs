namespace GoodOnYouScrapperAPI.Utils.HttpClientWrapper;

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
    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        return await _httpClient.GetAsync(requestUri);
    }
}