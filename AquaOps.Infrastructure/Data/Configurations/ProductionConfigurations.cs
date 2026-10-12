using AquaOps.Domain.Entities.Production;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AquaOps.Infrastructure.Data.Configurations;

public class ProductionOrderConfiguration : IEntityTypeConfiguration<ProductionOrder>
{
    public void Configure(EntityTypeBuilder<ProductionOrder> builder)
    {
        builder.ToTable("production_orders");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.PlannedQuantity).HasPrecision(18, 4);
        builder.Property(e => e.ActualQuantity).HasPrecision(18, 4);
        builder.Property(e => e.UnitOfMeasure).HasMaxLength(10);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.Notes).HasMaxLength(1000);
        builder.HasIndex(e => new { e.TenantId, e.OrderNumber }).IsUnique();
        builder.HasOne(e => e.Site).WithMany().HasForeignKey(e => e.SiteId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.OutputProduct).WithMany().HasForeignKey(e => e.OutputProductId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProductionBatchConfiguration : IEntityTypeConfiguration<ProductionBatch>
{
    public void Configure(EntityTypeBuilder<ProductionBatch> builder)
    {
        builder.ToTable("production_batches");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.BatchNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.Notes).HasMaxLength(1000);
        builder.HasIndex(e => new { e.TenantId, e.BatchNumber }).IsUnique();
        builder.HasOne(e => e.ProductionOrder).WithMany(o => o.Batches).HasForeignKey(e => e.ProductionOrderId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class BatchInputConfiguration : IEntityTypeConfiguration<BatchInput>
{
    public void Configure(EntityTypeBuilder<BatchInput> builder)
    {
        builder.ToTable("batch_inputs");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.WeightKg).HasPrecision(18, 4);
        builder.HasOne(e => e.ProductionBatch).WithMany(b => b.Inputs).HasForeignKey(e => e.ProductionBatchId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.InventoryItem).WithMany().HasForeignKey(e => e.InventoryItemId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class BatchOutputConfiguration : IEntityTypeConfiguration<BatchOutput>
{
    public void Configure(EntityTypeBuilder<BatchOutput> builder)
    {
        builder.ToTable("batch_outputs");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.WeightKg).HasPrecision(18, 4);
        builder.Property(e => e.LotNumber).HasMaxLength(50);
        builder.HasOne(e => e.ProductionBatch).WithMany(b => b.Outputs).HasForeignKey(e => e.ProductionBatchId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Product).WithMany().HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.QualityGrade).WithMany().HasForeignKey(e => e.QualityGradeId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class QualityGradeConfiguration : IEntityTypeConfiguration<QualityGrade>
{
    public void Configure(EntityTypeBuilder<QualityGrade> builder)
    {
        builder.ToTable("quality_grades");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Code).HasMaxLength(10).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
    }
}

public class QualityInspectionConfiguration : IEntityTypeConfiguration<QualityInspection>
{
    public void Configure(EntityTypeBuilder<QualityInspection> builder)
    {
        builder.ToTable("quality_inspections");
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Result).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.TemperatureCelsius).HasPrecision(5, 2);
        builder.Property(e => e.PhLevel).HasPrecision(4, 2);
        builder.Property(e => e.VisualCheck).HasMaxLength(500);
        builder.Property(e => e.SmellCheck).HasMaxLength(500);
        builder.Property(e => e.TextureCheck).HasMaxLength(500);
        builder.Property(e => e.Notes).HasMaxLength(1000);
        builder.HasOne(e => e.ProductionBatch).WithMany(b => b.QualityInspections).HasForeignKey(e => e.ProductionBatchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Inspector).WithMany().HasForeignKey(e => e.InspectorId).OnDelete(DeleteBehavior.Restrict);
    }
}