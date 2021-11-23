namespace TinyBacklog.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal identity)
    {
        return identity.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
            .Value ?? string.Empty;
    }

    public static string GetUserName(this ClaimsPrincipal identity)
    {
        return identity.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Name)?
            .Value ?? string.Empty;
    }
}
