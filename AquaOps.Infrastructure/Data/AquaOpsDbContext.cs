using AquaOps.Domain.Common;
using AquaOps.Domain.Entities.Core;
using AquaOps.Domain.Entities.Inventory;
using AquaOps.Domain.Entities.Production;
using AquaOps.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AquaOps.Infrastructure.Data;

public class AquaOpsDbContext : DbContext
{
    private readonly ICurrentUser _currentUser;

    public AquaOpsDbContext(DbContextOptions<AquaOpsDbContext> options, ICurrentUser currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    // ── Core ──
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<SyncLog> SyncLogs => Set<SyncLog>();
    public DbSet<Species> Species => Set<Species>();
    public DbSet<Product> Products => Set<Product>();

    // ── Inventory ──
    public DbSet<StorageLocation> StorageLocations => Set<StorageLocation>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
    public DbSet<TemperatureReading> TemperatureReadings => Set<TemperatureReading>();
    public DbSet<StockAlert> StockAlerts => Set<StockAlert>();

    // ── Production ──
    public DbSet<ProductionOrder> ProductionOrders => Set<ProductionOrder>();
    public DbSet<ProductionBatch> ProductionBatches => Set<ProductionBatch>();
    public DbSet<BatchInput> BatchInputs => Set<BatchInput>();
    public DbSet<BatchOutput> BatchOutputs => Set<BatchOutput>();
    public DbSet<QualityGrade> QualityGrades => Set<QualityGrade>();
    public DbSet<QualityInspection> QualityInspections => Set<QualityInspection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AquaOpsDbContext).Assembly);

        // ── Global tenant query filter ──
        // Automatically filters all TenantEntity queries by current user's tenant
        var tenantId = _currentUser.TenantId;

        modelBuilder.Entity<Site>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<AppUser>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<Role>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<RolePermission>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<Species>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<Product>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<StorageLocation>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<InventoryItem>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<StockAlert>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<ProductionOrder>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<ProductionBatch>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<BatchInput>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<BatchOutput>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<QualityGrade>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<QualityInspection>().HasQueryFilter(e => e.TenantId == tenantId);

        // Ledger entities filtered by tenant too
        modelBuilder.Entity<AuditLog>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<SyncLog>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<InventoryTransaction>().HasQueryFilter(e => e.TenantId == tenantId);
        modelBuilder.Entity<TemperatureReading>().HasQueryFilter(e => e.TenantId == tenantId);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow;
            var userId = _currentUser.UserId;
            var tenantId = _currentUser.TenantId;

            if (entry.Entity is BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    if (baseEntity.Id == Guid.Empty)
                        baseEntity.Id = Guid.NewGuid();
                    baseEntity.CreatedAt = now;
                    baseEntity.CreatedBy = userId;
                }
                baseEntity.UpdatedAt = now;
                baseEntity.UpdatedBy = userId;
            }

            if (entry.Entity is TenantEntity tenantEntity && entry.State == EntityState.Added)
            {
                if (tenantEntity.TenantId == Guid.Empty && tenantId.HasValue)
                    tenantEntity.TenantId = tenantId.Value;
            }

            if (entry.Entity is LedgerEntity ledgerEntity && entry.State == EntityState.Added)
            {
                ledgerEntity.CreatedAt = now;
                ledgerEntity.CreatedBy = userId;
                if (ledgerEntity.TenantId == Guid.Empty && tenantId.HasValue)
                    ledgerEntity.TenantId = tenantId.Value;
            }
        }
    }
}