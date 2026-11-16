namespace RateLimitLab;

public sealed record RateLimitResult(bool Allowed, double Remaining, long RetryAfter);
