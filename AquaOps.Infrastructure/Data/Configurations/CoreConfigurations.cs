using AquaOps.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AquaOps.Infrastructure.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Subdomain).HasMaxLength(63);
        builder.Property(e => e.Country).HasMaxLength(2);
        builder.Property(e => e.Currency).HasMaxLength(3);
        builder.Property(e => e.Timezone).HasMaxLength(50);
        builder.HasIndex(e => e.Subdomain).IsUnique().HasFilter("[Subdomain] IS NOT NULL");
    }
}

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("sites");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(20);
        builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(e => e.Address).HasMaxLength(500);
        builder.Property(e => e.City).HasMaxLength(100);
        builder.Property(e => e.Province).HasMaxLength(100);
        builder.Property(e => e.Country).HasMaxLength(2);
        builder.Property(e => e.Phone).HasMaxLength(20);
        builder.HasIndex(e => new { e.TenantId, e.Code }).IsUnique().HasFilter("[Code] IS NOT NULL");
        builder.HasOne(e => e.Tenant).WithMany(t => t.Sites).HasForeignKey(e => e.TenantId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("users");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Email).HasMaxLength(256).IsRequired();
        builder.Property(e => e.PasswordHash).HasMaxLength(256).IsRequired();
        builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Phone).HasMaxLength(20);
        builder.Property(e => e.PreferredLanguage).HasMaxLength(5);
        builder.Property(e => e.RefreshToken).HasMaxLength(256);
        builder.HasIndex(e => new { e.TenantId, e.Email }).IsUnique();
        builder.HasOne(e => e.Role).WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Site).WithMany().HasForeignKey(e => e.SiteId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.HasIndex(e => new { e.TenantId, e.Name }).IsUnique();
    }
}

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Module).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Action).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(200);
        builder.HasIndex(e => new { e.Module, e.Action }).IsUnique();
    }
}

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.HasOne(e => e.Role).WithMany(r => r.RolePermissions).HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Permission).WithMany(p => p.RolePermissions).HasForeignKey(e => e.PermissionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique();
    }
}

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");
        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.Action).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.EntityName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.EntityId).HasMaxLength(50);
        builder.Property(e => e.IpAddress).HasMaxLength(45);
        builder.HasIndex(e => new { e.TenantId, e.EntityName, e.CreatedAt });
    }
}

public class SyncLogConfiguration : IEntityTypeConfiguration<SyncLog>
{
    public void Configure(EntityTypeBuilder<SyncLog> builder)
    {
        builder.ToTable("sync_logs");
        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.EntityName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.EntityId).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(e => new { e.TenantId, e.DeviceId, e.Status });
    }
}

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Code).HasMaxLength(20).IsRequired();
        builder.Property(e => e.CommonName).HasMaxLength(200).IsRequired();
        builder.Property(e => e.ScientificName).HasMaxLength(200);
        builder.Property(e => e.LocalName).HasMaxLength(200);
        builder.Property(e => e.Category).HasMaxLength(50);
        builder.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Sku).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.UnitOfMeasure).HasMaxLength(10);
        builder.Property(e => e.StandardWeight).HasPrecision(18, 4);
        builder.Property(e => e.DefaultPricePerUnit).HasPrecision(18, 4);
        builder.Property(e => e.StorageInstructions).HasMaxLength(500);
        builder.HasIndex(e => new { e.TenantId, e.Sku }).IsUnique();
        builder.HasOne(e => e.Species).WithMany(s => s.Products).HasForeignKey(e => e.SpeciesId).OnDelete(DeleteBehavior.Restrict);
    }
}