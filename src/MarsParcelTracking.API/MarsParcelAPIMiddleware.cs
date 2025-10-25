using System.Diagnostics;

namespace MarsParcelTracking.API
{
    public class MarsParcelAPIMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MarsParcelAPIMiddleware> _logger;

        public MarsParcelAPIMiddleware(RequestDelegate next, ILogger<MarsParcelAPIMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving parcels");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An error occurred while processing your request.");
                return;
            }

            stopwatch.Stop();
            _logger.LogInformation($"Response: {context.Response.StatusCode} - {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
