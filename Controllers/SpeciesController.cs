using AquaOps.Application.DTOs.Core;
using AquaOps.Domain.Entities.Core;
using AquaOps.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AquaOps.API.Controllers;

[Authorize]
public class SpeciesController : BaseController
{
    private readonly IRepository<Species> _speciesRepo;
    private readonly IUnitOfWork _unitOfWork;

    public SpeciesController(IRepository<Species> speciesRepo, IUnitOfWork unitOfWork)
    {
        _speciesRepo = speciesRepo;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var species = await _speciesRepo.GetAllAsync();
        var dtos = species.Select(s => new SpeciesDto
        {
            Id = s.Id, Code = s.Code, CommonName = s.CommonName,
            ScientificName = s.ScientificName, LocalName = s.LocalName,
            Category = s.Category, IsActive = s.IsActive
        }).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var species = await _speciesRepo.GetByIdAsync(id);
        if (species == null) return NotFound(new { error = "Species not found" });
        return Ok(new SpeciesDto
        {
            Id = species.Id, Code = species.Code, CommonName = species.CommonName,
            ScientificName = species.ScientificName, LocalName = species.LocalName,
            Category = species.Category, IsActive = species.IsActive
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSpeciesRequest request)
    {
        var exists = await _speciesRepo.ExistsAsync(s => s.Code == request.Code);
        if (exists) return BadRequest(new { error = $"Species with code '{request.Code}' already exists" });

        var species = new Species
        {
            Code = request.Code, CommonName = request.CommonName,
            ScientificName = request.ScientificName, LocalName = request.LocalName,
            Category = request.Category
        };
        await _speciesRepo.AddAsync(species);
        await _unitOfWork.SaveChangesAsync();

        return StatusCode(201, new SpeciesDto
        {
            Id = species.Id, Code = species.Code, CommonName = species.CommonName,
            ScientificName = species.ScientificName, LocalName = species.LocalName,
            Category = species.Category, IsActive = species.IsActive
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSpeciesRequest request)
    {
        var species = await _speciesRepo.GetByIdAsync(id);
        if (species == null) return NotFound(new { error = "Species not found" });

        species.CommonName = request.CommonName;
        species.ScientificName = request.ScientificName;
        species.LocalName = request.LocalName;
        species.Category = request.Category;
        species.IsActive = request.IsActive;

        _speciesRepo.Update(species);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new SpeciesDto
        {
            Id = species.Id, Code = species.Code, CommonName = species.CommonName,
            ScientificName = species.ScientificName, LocalName = species.LocalName,
            Category = species.Category, IsActive = species.IsActive
        });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var species = await _speciesRepo.GetByIdAsync(id);
        if (species == null) return NotFound(new { error = "Species not found" });
        species.IsActive = false;
        _speciesRepo.Update(species);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
