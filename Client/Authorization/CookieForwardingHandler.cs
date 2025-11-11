using Client.Util;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Client.Authorization
{
    public class CookieForwardingHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CookieForwardingHandler> _logger;

        private readonly SemaphoreSlim _refreshSemaphore = new(1, 1);
        private bool _isRefreshing = false;

        public CookieForwardingHandler(ILogger<CookieForwardingHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            request.Headers.Add("X-Requested-With", ["XMLHttpRequest"]);

            _logger.LogInformation($"Sending request to: {request.RequestUri}");
            var response =  await base.SendAsync(request, cancellationToken);
            _logger.LogInformation($"Received response with status: {response.StatusCode}");

            return response;
        }
    }
}
