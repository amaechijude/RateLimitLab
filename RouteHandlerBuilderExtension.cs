namespace RateLimitLab;

public static class RouteHandlerBuilderExtension
{
    extension(RouteHandlerBuilder builder)
    {
        public RouteHandlerBuilder RequireIpRateLimit(string prefix) =>
            builder.AddEndpointFilter(
                async (context, next) =>
                {
                    var ip =
                        context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                    var limiter =
                        context.HttpContext.RequestServices.GetRequiredService<TokenBucket>();

                    var ct = context.HttpContext.RequestAborted;

                    var result = await limiter.AllowAsync($"{prefix}:{ip}", ct);
                    if (!result.Allowed)
                    {
                        context.HttpContext.Response.Headers.RetryAfter =
                            result.RetryAfter.ToString();

                        return Results.Problem(statusCode: StatusCodes.Status429TooManyRequests);
                    }
                    return await next(context);
                }
            );
    }
}
