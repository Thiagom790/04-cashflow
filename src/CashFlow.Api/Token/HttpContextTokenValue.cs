using CashFlow.Domain.Security.Tokens;

namespace CashFlow.Api.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string TokenOnRequest()
    {
        var autorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        //Bearer abc123
        // pego a partir da posição 7
        return autorization["Bearer ".Length..].Trim();
    }
}