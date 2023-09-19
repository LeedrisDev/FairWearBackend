using AutoMapper;
using BackOffice.DataAccess.Entities;
using BackOffice.DataAccess.Repositories.Interfaces;
using BackOffice.Models;

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
}
