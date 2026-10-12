using AquaOps.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AquaOps.Infrastructure.Data.Configurations;

public class StorageLocationConfiguration : IEntityTypeConfiguration<StorageLocation>
{
    public void Configure(EntityTypeBuilder<StorageLocation> builder)
    {
        builder.ToTable("storage_locations");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(20);
        builder.Property(e => e.Zone).HasMaxLength(50);
        builder.Property(e => e.TemperatureMin).HasPrecision(5, 2);
        builder.Property(e => e.TemperatureMax).HasPrecision(5, 2);
        builder.HasIndex(e => new { e.TenantId, e.SiteId, e.Code }).IsUnique().HasFilter("[Code] IS NOT NULL");
        builder.HasOne(e => e.Site).WithMany().HasForeignKey(e => e.SiteId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("inventory_items");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.LotNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.BatchNumber).HasMaxLength(50);
        builder.Property(e => e.WeightKg).HasPrecision(18, 4);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.UnitCost).HasPrecision(18, 4);
        builder.Property(e => e.ReferenceType).HasMaxLength(50);
        builder.Property(e => e.ReferenceId).HasMaxLength(50);

        // FEFO composite index — powers "what expires first?" queries
        builder.HasIndex(e => new { e.TenantId, e.ProductId, e.Status, e.ExpiryDate })
            .HasDatabaseName("IX_inventory_items_fefo");

        builder.HasOne(e => e.Product).WithMany().HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.StorageLocation).WithMany(sl => sl.InventoryItems).HasForeignKey(e => e.StorageLocationId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.ToTable("inventory_transactions");
        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.WeightKg).HasPrecision(18, 4);
        builder.Property(e => e.ReferenceType).HasMaxLength(50);
        builder.Property(e => e.ReferenceId).HasMaxLength(50);
        builder.Property(e => e.Notes).HasMaxLength(500);
        builder.HasIndex(e => new { e.TenantId, e.InventoryItemId, e.CreatedAt });
        builder.HasOne(e => e.InventoryItem).WithMany(i => i.Transactions).HasForeignKey(e => e.InventoryItemId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.FromLocation).WithMany().HasForeignKey(e => e.FromLocationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.ToLocation).WithMany().HasForeignKey(e => e.ToLocationId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class TemperatureReadingConfiguration : IEntityTypeConfiguration<TemperatureReading>
{
    public void Configure(EntityTypeBuilder<TemperatureReading> builder)
    {
        builder.ToTable("temperature_readings");
        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.TemperatureCelsius).HasPrecision(5, 2);
        builder.Property(e => e.HumidityPercent).HasPrecision(5, 2);
        builder.Property(e => e.RecordedByDevice).HasMaxLength(100);
        builder.HasIndex(e => new { e.TenantId, e.StorageLocationId, e.CreatedAt });
        builder.HasOne(e => e.StorageLocation).WithMany(sl => sl.TemperatureReadings).HasForeignKey(e => e.StorageLocationId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class StockAlertConfiguration : IEntityTypeConfiguration<StockAlert>
{
    public void Configure(EntityTypeBuilder<StockAlert> builder)
    {
        builder.ToTable("stock_alerts");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(e => e.Severity).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.Message).HasMaxLength(500).IsRequired();
        builder.HasIndex(e => new { e.TenantId, e.IsAcknowledged, e.Severity });
        builder.HasOne(e => e.Product).WithMany().HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.StorageLocation).WithMany().HasForeignKey(e => e.StorageLocationId).OnDelete(DeleteBehavior.Restrict);
    }
}