using System.Net;
using AutoMapper;
using BrandAndProduct.Service.DataAccess.IRepositories;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProduct.Service.DataAccess.Repositories;

/// <summary>Class representing a repository for the BrandEntity.</summary>
public class BrandRepository : Repository<BrandDto, BrandEntity>, IBrandRepository
{
    /// <summary>Constructor for BrandRepository</summary>
    public BrandRepository(BrandAndProductDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> GetBrandByNameAsync(string name)
    {
        var processingStatusResponse = new ProcessingStatusResponse<BrandDto>();

        var brand = await DbSet.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        if (brand == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Brand with name {name} was not found.";
            return processingStatusResponse;
        }

        processingStatusResponse.Object = Mapper.Map<BrandDto>(brand);
        return processingStatusResponse;
    }
}