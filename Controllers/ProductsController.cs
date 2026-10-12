using AquaOps.Application.DTOs.Core;
using AquaOps.Domain.Entities.Core;
using AquaOps.Domain.Interfaces;
using AquaOps.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquaOps.API.Controllers;

[Authorize]
public class ProductsController : BaseController
{
    private readonly AquaOpsDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(AquaOpsDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products.Include(p => p.Species).ToListAsync();
        var dtos = products.Select(p => new ProductDto
        {
            Id = p.Id, Sku = p.Sku, Name = p.Name, Description = p.Description,
            SpeciesId = p.SpeciesId, SpeciesName = p.Species.CommonName,
            UnitOfMeasure = p.UnitOfMeasure, IsCatchWeight = p.IsCatchWeight,
            StandardWeight = p.StandardWeight, DefaultPricePerUnit = p.DefaultPricePerUnit,
            ShelfLifeDays = p.ShelfLifeDays, ReorderLevel = p.ReorderLevel,
            StorageInstructions = p.StorageInstructions, IsActive = p.IsActive
        }).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _context.Products.Include(p => p.Species).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound(new { error = "Product not found" });
        return Ok(new ProductDto
        {
            Id = product.Id, Sku = product.Sku, Name = product.Name, Description = product.Description,
            SpeciesId = product.SpeciesId, SpeciesName = product.Species.CommonName,
            UnitOfMeasure = product.UnitOfMeasure, IsCatchWeight = product.IsCatchWeight,
            StandardWeight = product.StandardWeight, DefaultPricePerUnit = product.DefaultPricePerUnit,
            ShelfLifeDays = product.ShelfLifeDays, ReorderLevel = product.ReorderLevel,
            StorageInstructions = product.StorageInstructions, IsActive = product.IsActive
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var skuExists = await _context.Products.AnyAsync(p => p.Sku == request.Sku);
        if (skuExists) return BadRequest(new { error = $"Product with SKU '{request.Sku}' already exists" });
        var speciesExists = await _context.Species.AnyAsync(s => s.Id == request.SpeciesId);
        if (!speciesExists) return BadRequest(new { error = "Species not found" });

        var product = new Product
        {
            Sku = request.Sku, Name = request.Name, Description = request.Description,
            SpeciesId = request.SpeciesId, UnitOfMeasure = request.UnitOfMeasure,
            IsCatchWeight = request.IsCatchWeight, StandardWeight = request.StandardWeight,
            DefaultPricePerUnit = request.DefaultPricePerUnit, ShelfLifeDays = request.ShelfLifeDays,
            ReorderLevel = request.ReorderLevel, StorageInstructions = request.StorageInstructions
        };
        await _context.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        await _context.Entry(product).Reference(p => p.Species).LoadAsync();

        return StatusCode(201, new ProductDto
        {
            Id = product.Id, Sku = product.Sku, Name = product.Name, Description = product.Description,
            SpeciesId = product.SpeciesId, SpeciesName = product.Species.CommonName,
            UnitOfMeasure = product.UnitOfMeasure, IsCatchWeight = product.IsCatchWeight,
            StandardWeight = product.StandardWeight, DefaultPricePerUnit = product.DefaultPricePerUnit,
            ShelfLifeDays = product.ShelfLifeDays, ReorderLevel = product.ReorderLevel,
            StorageInstructions = product.StorageInstructions, IsActive = product.IsActive
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound(new { error = "Product not found" });
        var speciesExists = await _context.Species.AnyAsync(s => s.Id == request.SpeciesId);
        if (!speciesExists) return BadRequest(new { error = "Species not found" });

        product.Name = request.Name; product.Description = request.Description;
        product.SpeciesId = request.SpeciesId; product.UnitOfMeasure = request.UnitOfMeasure;
        product.IsCatchWeight = request.IsCatchWeight; product.StandardWeight = request.StandardWeight;
        product.DefaultPricePerUnit = request.DefaultPricePerUnit; product.ShelfLifeDays = request.ShelfLifeDays;
        product.ReorderLevel = request.ReorderLevel; product.StorageInstructions = request.StorageInstructions;
        product.IsActive = request.IsActive;
        await _unitOfWork.SaveChangesAsync();
        await _context.Entry(product).Reference(p => p.Species).LoadAsync();

        return Ok(new ProductDto
        {
            Id = product.Id, Sku = product.Sku, Name = product.Name, Description = product.Description,
            SpeciesId = product.SpeciesId, SpeciesName = product.Species.CommonName,
            UnitOfMeasure = product.UnitOfMeasure, IsCatchWeight = product.IsCatchWeight,
            StandardWeight = product.StandardWeight, DefaultPricePerUnit = product.DefaultPricePerUnit,
            ShelfLifeDays = product.ShelfLifeDays, ReorderLevel = product.ReorderLevel,
            StorageInstructions = product.StorageInstructions, IsActive = product.IsActive
        });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound(new { error = "Product not found" });
        product.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
