namespace Ticketing.Client.NswagAutoGen.HttpClientFactoryImplementation;
public abstract class ClientBase
{
  protected abstract string HttpClientName { get; }

  private readonly IHttpClientFactory _httpClientFactory;

  public ClientBase(IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory;
  }

  protected virtual Task<HttpClient> CreateHttpClientAsync(CancellationToken token = default)
  {
    return Task.FromResult(_httpClientFactory.CreateClient(HttpClientName));
  }
}