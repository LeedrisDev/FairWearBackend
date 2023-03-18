namespace GoodOnYouScrapperAPI.Utils.HttpClientWrapper;

public class HttpClientWrapper: IHttpClientWrapper
{
    private readonly HttpClient _httpClient;
    
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