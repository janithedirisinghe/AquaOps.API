using System.Security.Claims;
using AquaOps.Application.DTOs.Auth;
using AquaOps.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AquaOps.API.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly ITenantService _tenantService;

    public AuthController(IAuthService authService, ITenantService tenantService)
    {
        _authService = authService;
        _tenantService = tenantService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] TenantRegistrationRequest request)
    {
        var result = await _tenantService.RegisterTenantAsync(request);
        return CreatedFromResult(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return FromResult(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return FromResult(result);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _authService.ChangePasswordAsync(userId, request);
        return FromResult(result);
    }

    [HttpPost("revoke")]
    [Authorize]
    public async Task<IActionResult> RevokeToken()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _authService.RevokeTokenAsync(userId);
        return FromResult(result);
    }
}
