using System.Net;
using AutoMapper;
using BackOffice.DataAccess.Entities;
using BackOffice.DataAccess.Repositories.Interfaces;
using BackOffice.Models;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.DataAccess.Repositories.Classes;

/// <summary>Represents a repository implementation for managing brand entities.</summary>
public class BrandRepository : Repository<BrandModel, BrandEntity>, IBrandRepository
{
    /// <summary>Initializes a new instance of the <see cref="BrandRepository"/> class.</summary>
    /// <param name="context">The database context used for data access operations.</param>
    /// <param name="logger">The logger for logging repository-related activities.</param>
    /// <param name="mapper">The mapper used for mapping between <see cref="BrandModel"/> and <see cref="BrandEntity"/>.</param>
    public BrandRepository(BrandAndProductDbContext context, ILogger<Repository<BrandModel, BrandEntity>> logger, IMapper mapper) 
        : base(context, logger, mapper) { }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandModel>> FindByNameAsync(string name)
    {
        var response = new ProcessingStatusResponse<BrandModel>();

        try
        {
            var entity = await Set
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Name == name);

            if (entity == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = $"No brand with name '{name}' was found.";
                return response;
            }
            
            response.Entity = Mapper.Map<BrandModel>(entity);
        }
        catch (Exception e)
        {
            Logger.LogError("Error on database {Exception}", e.ToString());
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Message = "Internal server error.";
        }
        
        return response;
    }
}
