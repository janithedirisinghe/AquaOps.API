using System.Security.Claims;
using AquaOps.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AquaOps.Infrastructure.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return id != null ? Guid.Parse(id) : null;
        }
    }

    public Guid? TenantId
    {
        get
        {
            var tid = User?.FindFirst("tenant_id")?.Value;
            return tid != null ? Guid.Parse(tid) : null;
        }
    }

    public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;

    public IEnumerable<string> Permissions =>
        User?.FindAll("permission")?.Select(c => c.Value) ?? Enumerable.Empty<string>();
}
