using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Ticketing.BFF.Infrastructure.Http;
public class PropagateBearerTokenHandler : DelegatingHandler
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public PropagateBearerTokenHandler(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    var context = _httpContextAccessor.HttpContext;

    var token = context?.Items["ManualToken"] as string;
    if (string.IsNullOrEmpty(token))
    {
      var authHeader = context?.Request.Headers["Authorization"].FirstOrDefault();
      if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        token = authHeader.Substring("Bearer ".Length);
    }

    if (!string.IsNullOrEmpty(token))
      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

    return await base.SendAsync(request, cancellationToken);
  }
}
