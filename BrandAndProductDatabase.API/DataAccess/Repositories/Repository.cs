using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProductDatabase.API.DataAccess.Repositories;

/// <summary>Class representing a generic repository.</summary>
public class Repository<TModel, TEntity> : IRepository<TModel>
    where TModel : class, IObjectWithId
    where TEntity : class, IObjectWithId
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="Repository{TModel,TEntity}"/> class.</summary>
    /// <param name="context">The <see cref="DbContext"/> instance used to access the data store.</param>
    /// <param name="mapper">
    /// The <see cref="IMapper"/> instance used to map between <typeparamref name="TModel"/> and <typeparamref name="TEntity"/>.
    /// </param>
    public Repository(DbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _mapper = mapper;
    }
    
    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<TModel>>> GetAllAsync()
    {
        var processingStatusResponse = new ProcessingStatusResponse<IEnumerable<TModel>>();
        
        var entities = await _dbSet.ToListAsync();
        processingStatusResponse.Object = _mapper.Map<IEnumerable<TModel>>(entities);

        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<TModel>> GetByIdAsync(int id)
    {
        var processingStatusResponse = new ProcessingStatusResponse<TModel>();
        
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Entity with ID {id} not found.";
            return processingStatusResponse;
        }
        
        processingStatusResponse.Object = _mapper.Map<TModel>(entity);
        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<TModel>> AddAsync(TModel entity)
    {
        var processingStatusResponse = new ProcessingStatusResponse<TModel>();
        
        var entityToAdd = _mapper.Map<TEntity>(entity);
        await _dbSet.AddAsync(entityToAdd);
        await _context.SaveChangesAsync();
        
        processingStatusResponse.Object = _mapper.Map<TModel>(entityToAdd);
        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<TModel>> UpdateAsync(TModel entity)
    {
        var processingStatusResponse = new ProcessingStatusResponse<TModel>();
        var entityToUpdate = await _dbSet.FindAsync(entity.Id);

        if (entityToUpdate == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Entity with ID {entity.Id} not found.";
            return processingStatusResponse;
        }
        
        var entityUpdated = _dbSet.Update(_mapper.Map<TEntity>(entity));
        await _context.SaveChangesAsync();
        
        processingStatusResponse.Object = _mapper.Map<TModel>(entityUpdated);
        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<TModel>> DeleteAsync(int id)
    {
        var processingStatusResponse = new ProcessingStatusResponse<TModel>();
        var entityToDelete = await _dbSet.FindAsync(id);

        if (entityToDelete == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Entity with ID {id} not found.";
            return processingStatusResponse;
        }
        
        _dbSet.Remove(entityToDelete);
        processingStatusResponse.Object = _mapper.Map<TModel>(entityToDelete);
        await _context.SaveChangesAsync();

        return processingStatusResponse;
    }
}