using AquaOps.Application.Common;
using AquaOps.Application.DTOs.Auth;
using AquaOps.Application.Interfaces;
using AquaOps.Domain.Entities.Core;
using AquaOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AquaOps.Infrastructure.Auth;

public class TenantService : ITenantService
{
    private readonly AquaOpsDbContext _context;
    private readonly JwtTokenService _tokenService;

    public TenantService(AquaOpsDbContext context, JwtTokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponse>> RegisterTenantAsync(TenantRegistrationRequest request)
    {
        var existingUser = await _context.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.Email == request.AdminEmail);

        if (existingUser)
            return Result<AuthResponse>.Failure("Email already registered");

        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.TenantName,
            Country = request.Country,
            Currency = request.Currency,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Tenants.AddAsync(tenant);

        var permissionDefinitions = new[]
        {
            ("Core", "ManageTenants"), ("Core", "ManageSites"), ("Core", "ManageUsers"), ("Core", "ManageRoles"),
            ("Core", "ViewAuditLog"), ("Core", "ManageSpecies"), ("Core", "ManageProducts"),
            ("Inventory", "ViewInventory"), ("Inventory", "ManageInventory"), ("Inventory", "ManageLocations"),
            ("Inventory", "RecordTemperature"), ("Inventory", "ManageAlerts"),
            ("Production", "ViewOrders"), ("Production", "ManageOrders"), ("Production", "ManageBatches"),
            ("Production", "PerformInspection"), ("Production", "ManageGrades")
        };

        var permissions = new List<Permission>();
        foreach (var (module, action) in permissionDefinitions)
        {
            var perm = await _context.Permissions
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Module == module && p.Action == action);

            if (perm == null)
            {
                perm = new Permission
                {
                    Id = Guid.NewGuid(),
                    Module = module,
                    Action = action,
                    Description = $"{action} in {module} module",
                    CreatedAt = DateTime.UtcNow
                };
                await _context.Permissions.AddAsync(perm);
            }
            permissions.Add(perm);
        }

        var adminRole = new Role
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Name = "Admin",
            Description = "Full system access",
            IsSystemRole = true,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Roles.AddAsync(adminRole);

        foreach (var perm in permissions)
        {
            await _context.RolePermissions.AddAsync(new RolePermission
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                RoleId = adminRole.Id,
                PermissionId = perm.Id,
                CreatedAt = DateTime.UtcNow
            });
        }

        var adminUser = new AppUser
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Email = request.AdminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.AdminPassword, workFactor: 12),
            FirstName = request.AdminFirstName,
            LastName = request.AdminLastName,
            RoleId = adminRole.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Users.AddAsync(adminUser);

        await _context.SaveChangesAsync();

        var permissionStrings = permissions.Select(p => $"{p.Module}.{p.Action}").ToList();
        var token = _tokenService.GenerateToken(adminUser, adminRole, permissionStrings);
        var refreshToken = _tokenService.GenerateRefreshToken();

        adminUser.RefreshToken = refreshToken;
        adminUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        adminUser.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Result<AuthResponse>.Created(new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiry = DateTime.UtcNow.AddMinutes(60),
            UserId = adminUser.Id,
            Email = adminUser.Email,
            FullName = $"{adminUser.FirstName} {adminUser.LastName}",
            Role = adminRole.Name,
            Permissions = permissionStrings
        });
    }
}
