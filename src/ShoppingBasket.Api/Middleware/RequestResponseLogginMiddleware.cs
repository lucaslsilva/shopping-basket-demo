namespace ShoppingBasket.Api.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request
            _logger.LogInformation("HTTP {Method} {Path}", context.Request.Method, context.Request.Path);

            // Copy original response body to read it
            var originalBodyStream = context.Response.Body;

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context); // process the request

                // Log the response
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation("Response {StatusCode}: {Body}", context.Response.StatusCode, text);

                await responseBody.CopyToAsync(originalBodyStream); // write back to original stream
            }
            finally
            {
                context.Response.Body = originalBodyStream; // restore original stream
            }
        }
    }
}
