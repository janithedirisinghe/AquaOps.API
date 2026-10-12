namespace AquaOps.Application.DTOs.Core;

// ── Species DTOs ──

public class SpeciesDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string CommonName { get; set; } = string.Empty;
    public string? ScientificName { get; set; }
    public string? LocalName { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSpeciesRequest
{
    public string Code { get; set; } = string.Empty;
    public string CommonName { get; set; } = string.Empty;
    public string? ScientificName { get; set; }
    public string? LocalName { get; set; }
    public string? Category { get; set; }
}

public class UpdateSpeciesRequest
{
    public string CommonName { get; set; } = string.Empty;
    public string? ScientificName { get; set; }
    public string? LocalName { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; }
}

// ── Product DTOs ──

public class ProductDto
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid SpeciesId { get; set; }
    public string? SpeciesName { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public bool IsCatchWeight { get; set; }
    public decimal? StandardWeight { get; set; }
    public decimal? DefaultPricePerUnit { get; set; }
    public int? ShelfLifeDays { get; set; }
    public int? ReorderLevel { get; set; }
    public string? StorageInstructions { get; set; }
    public bool IsActive { get; set; }
}

public class CreateProductRequest
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid SpeciesId { get; set; }
    public string UnitOfMeasure { get; set; } = "kg";
    public bool IsCatchWeight { get; set; }
    public decimal? StandardWeight { get; set; }
    public decimal? DefaultPricePerUnit { get; set; }
    public int? ShelfLifeDays { get; set; }
    public int? ReorderLevel { get; set; }
    public string? StorageInstructions { get; set; }
}

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid SpeciesId { get; set; }
    public string UnitOfMeasure { get; set; } = "kg";
    public bool IsCatchWeight { get; set; }
    public decimal? StandardWeight { get; set; }
    public decimal? DefaultPricePerUnit { get; set; }
    public int? ShelfLifeDays { get; set; }
    public int? ReorderLevel { get; set; }
    public string? StorageInstructions { get; set; }
    public bool IsActive { get; set; }
}