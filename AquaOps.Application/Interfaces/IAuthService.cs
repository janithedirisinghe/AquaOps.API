using AquaOps.Application.Common;
using AquaOps.Application.DTOs.Auth;

namespace AquaOps.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
    Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
    Task<Result> RevokeTokenAsync(Guid userId);
}

public interface ITenantService
{
    Task<Result<AuthResponse>> RegisterTenantAsync(TenantRegistrationRequest request);
}
