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
    /// <summary>The <see cref="DbContext"/> instance used to access the data store.</summary>
    protected readonly BrandAndProductDbContext Context;
    /// <summary>The <see cref="DbSet{TEntity}"/> instance used to access the data store.</summary>
    protected readonly DbSet<TEntity> DbSet;
    /// <summary>
    /// The <see cref="IMapper"/> instance used to map between <typeparamref name="TModel"/> and <typeparamref name="TEntity"/>.
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>Initializes a new instance of the <see cref="Repository{TModel,TEntity}"/> class.</summary>
    /// <param name="context">The <see cref="DbContext"/> instance used to access the data store.</param>
    /// <param name="mapper">
    /// The <see cref="IMapper"/> instance used to map between <typeparamref name="TModel"/> and <typeparamref name="TEntity"/>.
    /// </param>
    protected Repository(BrandAndProductDbContext context, IMapper mapper)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
        Mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<TModel>>> GetAllAsync()
    {
        var processingStatusResponse = new ProcessingStatusResponse<IEnumerable<TModel>>();

        var entities = await DbSet.ToListAsync();
        processingStatusResponse.Object = Mapper.Map<IEnumerable<TModel>>(entities);

        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<TModel>> GetByIdAsync(int id)
    {
        var processingStatusResponse = new ProcessingStatusResponse<TModel>();

        var entity = await DbSet.FindAsync(id);
        if (entity == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Entity with ID {id} not found.";
            return processingStatusResponse;
        }

        processingStatusResponse.Object = Mapper.Map<TModel>(entity);
        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<TModel>> AddAsync(TModel entity)
    {
        var processingStatusResponse = new ProcessingStatusResponse<TModel>();

        var entityToAdd = Mapper.Map<TEntity>(entity);
        await DbSet.AddAsync(entityToAdd);
        await Context.SaveChangesAsync();

        processingStatusResponse.Object = Mapper.Map<TModel>(entityToAdd);
        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<TModel>> UpdateAsync(TModel entity)
    {
        var processingStatusResponse = new ProcessingStatusResponse<TModel>();
        var entityToUpdate = await DbSet.FindAsync(entity.Id);

        if (entityToUpdate == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Entity with ID {entity.Id} not found.";
            return processingStatusResponse;
        }

        Mapper.Map(entity, entityToUpdate);
        DbSet.Update(entityToUpdate);
        await Context.SaveChangesAsync();

        processingStatusResponse.Object = Mapper.Map<TModel>(entityToUpdate);
        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<TModel>> DeleteAsync(int id)
    {
        var processingStatusResponse = new ProcessingStatusResponse<TModel>();
        var entityToDelete = await DbSet.FindAsync(id);

        if (entityToDelete == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Entity with ID {id} not found.";
            return processingStatusResponse;
        }

        DbSet.Remove(entityToDelete);
        processingStatusResponse.Object = Mapper.Map<TModel>(entityToDelete);
        await Context.SaveChangesAsync();

        return processingStatusResponse;
    }
}