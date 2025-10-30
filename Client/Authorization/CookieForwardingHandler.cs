namespace Client.Authorization
{
    public class CookieForwardingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieForwardingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var cookies = httpContext.Request.Cookies;
                if (cookies.Any())
                {
                    var cookieHeader = string.Join("; ",
                        cookies.Select(c => $"{c.Key}={c.Value}"));
                    request.Headers.Add("Cookie", cookieHeader);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
