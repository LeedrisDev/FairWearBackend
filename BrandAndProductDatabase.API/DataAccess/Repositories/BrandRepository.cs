using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProductDatabase.API.DataAccess.Repositories;

/// <summary>Class representing a repository for the BrandEntity.</summary>
public class BrandRepository : Repository<BrandDto, BrandEntity>, IBrandRepository
{
    /// <summary>Constructor for BrandRepository</summary>
    public BrandRepository(DbContext context, IMapper mapper) : base(context, mapper) { }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> GetBrandByNameAsync(string name)
    {
        var processingStatusResponse = new ProcessingStatusResponse<BrandDto>();
        
        var brand = await DbSet.FirstOrDefaultAsync(x => x.Name == name);
        if (brand == null)
        {
            return new ProcessingStatusResponse<BrandDto>
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = $"Brand with name {name} not found."
            };
        }
        
        processingStatusResponse.Object = Mapper.Map<BrandDto>(brand);
        return processingStatusResponse;
    }
}