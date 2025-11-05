using Client.Util;
using System.Net;

namespace Client.Authorization
{
    public class CookieForwardingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthService _authService;

        private readonly SemaphoreSlim _refreshSemaphore = new(1, 1);
        private bool _isRefreshing = false;

        public CookieForwardingHandler(IHttpContextAccessor httpContextAccessor, AuthService authService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var cookies = httpContext.Request.Cookies;
                Console.WriteLine("Forwarding cookies:");
                Console.WriteLine(string.Join(", ", cookies.Select(c => $"{c.Key}={c.Value}")));
                if (cookies.Any())
                {
                    var cookieHeader = string.Join("; ",
                        cookies.Select(c => $"{c.Key}={c.Value}"));
                    request.Headers.Add("Cookie", cookieHeader);
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            return response;

            // TODO: Implement lazy token refresh logic - intercept 401 responses and attempt to refresh tokens

            //if (response.StatusCode != HttpStatusCode.Unauthorized)
            //{
            //    Console.WriteLine("Request authorized, no need to refresh token.");
            //    return response;
            //}

            //if (request.RequestUri != null && request.RequestUri.AbsolutePath.EndsWith("/api/auth/refresh-token"))
            //{
            //    Console.WriteLine("Refresh token request failed, user needs to log in again.");
            //    return response;
            //}

            //await _refreshSemaphore.WaitAsync(cancellationToken);
            //try
            //{
            //    // If another request already refreshed, retry immediately
            //    if (_isRefreshing)
            //    {
            //        return await base.SendAsync(request, cancellationToken);
            //    }

            //    _isRefreshing = true;

            //    // Attempt refresh
            //    var refreshSuccess = await _authService.RefreshCookies();

            //    if (!refreshSuccess)
            //    {
            //        // Refresh failed, user needs to log in again
            //        return response;
            //    }

            //    // Retry original request with new token
            //    var retryResponse = await base.SendAsync(request, cancellationToken);
            //    return retryResponse;
            //}
            //finally
            //{
            //    _isRefreshing = false;
            //    _refreshSemaphore.Release();
            //}
        }
    }
}
