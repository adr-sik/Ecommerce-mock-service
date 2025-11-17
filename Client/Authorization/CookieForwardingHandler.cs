using Client.Util;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Net;

namespace Client.Authorization
{
    public class CookieForwardingHandler : DelegatingHandler
    {
        private readonly ILogger<CookieForwardingHandler> _logger;
        private readonly IJSRuntime _jsRuntime;

        public CookieForwardingHandler(ILogger<CookieForwardingHandler> logger, IJSRuntime jsRuntime)
        {
            _logger = logger;
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var token = await _jsRuntime.InvokeAsync<string>("getCookie", "XSRF-TOKEN");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("X-CSRF", token);
            }

            _logger.LogInformation($"Sending request to: {request.RequestUri}");
            var response =  await base.SendAsync(request, cancellationToken);
            _logger.LogInformation($"Received response with status: {response.StatusCode}");

            return response;
        }
    }
}
