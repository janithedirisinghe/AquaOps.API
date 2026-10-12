using AquaOps.Application.Common;
using AquaOps.Application.DTOs.Auth;
using AquaOps.Application.Interfaces;
using AquaOps.Domain.Interfaces;
using AquaOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AquaOps.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly AquaOpsDbContext _context;
    private readonly JwtTokenService _tokenService;

    public AuthService(AquaOpsDbContext context, JwtTokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .IgnoreQueryFilters()
            .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure("Invalid email or password", 401);

        var permissions = user.Role.RolePermissions
            .Select(rp => $"{rp.Permission.Module}.{rp.Permission.Action}")
            .ToList();

        var token = _tokenService.GenerateToken(user, user.Role, permissions);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Result<AuthResponse>.Success(new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiry = DateTime.UtcNow.AddMinutes(60),
            UserId = user.Id,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role.Name,
            Permissions = permissions
        });
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
        if (principal == null)
            return Result<AuthResponse>.Failure("Invalid token", 401);

        var userId = Guid.Parse(principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        var user = await _context.Users
            .IgnoreQueryFilters()
            .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
            return Result<AuthResponse>.Failure("Invalid or expired refresh token", 401);

        var permissions = user.Role.RolePermissions
            .Select(rp => $"{rp.Permission.Module}.{rp.Permission.Action}")
            .ToList();

        var newToken = _tokenService.GenerateToken(user, user.Role, permissions);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return Result<AuthResponse>.Success(new AuthResponse
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            Expiry = DateTime.UtcNow.AddMinutes(60),
            UserId = user.Id,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role.Name,
            Permissions = permissions
        });
    }

    public async Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        var user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return Result.NotFound("User not found");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return Result.Failure("Current password is incorrect");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, workFactor: 12);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> RevokeTokenAsync(Guid userId)
    {
        var user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return Result.NotFound("User not found");

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
