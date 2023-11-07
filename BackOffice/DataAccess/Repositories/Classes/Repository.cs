using System.Net;
using AutoMapper;
using BackOffice.DataAccess.Repositories.Interfaces;
using BackOffice.Models;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.DataAccess.Repositories.Classes;

/// <summary>
/// Generic repository implementation for data access operations on entities of type <typeparamref name="TModel"/>
/// using entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TModel">The type of model entity returned or operated upon by the repository.</typeparam>
/// <typeparam name="TEntity">The type of entity stored in the data store.</typeparam>
public class Repository<TModel, TEntity> : IRepository<TModel>
    where TModel : class, IObjectWithId, new()
    where TEntity : class, IObjectWithId, new()
{
    /// <summary>The database set representing the collection of <typeparamref name="TEntity"/> in the context.</summary>
    protected DbSet<TEntity> Set;

    /// <summary>The database context used for data access operations.</summary>
    protected BrandAndProductDbContext Context;

    /// <summary>The logger for logging repository-related activities.</summary>
    protected ILogger Logger;

    /// <summary>The mapper used for mapping between <typeparamref name="TModel"/> and <typeparamref name="TEntity"/>.</summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TModel, TEntity}"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access operations.</param>
    /// <param name="logger">The logger for logging repository-related activities.</param>
    /// <param name="mapper">The mapper used for mapping between <typeparamref name="TModel"/> and <typeparamref name="TEntity"/>.</param>
    public Repository(BrandAndProductDbContext context, ILogger<Repository<TModel, TEntity>> logger, IMapper mapper)
    {
        Set = context.Set<TEntity>();
        Context = context;
        Logger = logger;
        Mapper = mapper;
    }

    /// <inheritdoc/>
    public virtual async Task<ProcessingStatusResponse<IEnumerable<TModel>>> GetAsync(string includeTables = "")
    {
        var response = new ProcessingStatusResponse<IEnumerable<TModel>>();
        try
        {
            IEnumerable<TEntity> entities;
            if (string.IsNullOrEmpty(includeTables))
                entities = await Set
                    .AsNoTracking()
                    .OrderBy(e => e.Id)
                    .ToListAsync();
            else
                entities = await Set
                    .AsNoTracking()
                    .Include(includeTables)
                    .OrderBy(e => e.Id)
                    .ToListAsync();

            response.Entity = Mapper.Map<IEnumerable<TModel>>(entities);
        }
        catch (Exception e)
        {
            Logger.LogError("Error on database {Exception}", e.ToString());
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Message = "Internal server error.";
        }
        
        return response;
    }

    /// <inheritdoc/>
    public virtual async Task<ProcessingStatusResponse<TModel>> InsertAsync(TModel entity)
    {
        var response = new ProcessingStatusResponse<TModel>();
        var entityToInsert = Mapper.Map<TEntity>(entity);
        Set.Add(entityToInsert);

        try
        {
            await Context.SaveChangesAsync();
            response.Entity = Mapper.Map<TModel>(entityToInsert);
            response.StatusCode = HttpStatusCode.Created;
        }
        catch (Exception e)
        {
            Logger.LogError("Error on database {Exception}", e.ToString());
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Message = "Internal server error.";
        }
        
        return response;
    }

    /// <inheritdoc/>
    public virtual async Task<ProcessingStatusResponse<TModel>> UpdateAsync(TModel entity)
    {
        var response = new ProcessingStatusResponse<TModel>();
        var databaseEntity = await Set.FindAsync(entity.Id);

        if (databaseEntity == null)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = "Entity not found.";
            return response;
        }

        Mapper.Map(entity, databaseEntity);

        try
        {
            await Context.SaveChangesAsync();
            response.Entity = Mapper.Map<TModel>(databaseEntity);
        }
        catch (Exception e)
        {
            Logger.LogError("Error on database {Exception}", e.ToString());
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Message = "Internal server error.";
        }

        return response;
    }

    /// <inheritdoc/>
    public virtual async Task<ProcessingStatusResponse<TModel>> DeleteAsync(long id)
    {
        var response = new ProcessingStatusResponse<TModel>();
        var entityToDelete = await Set.FindAsync(id);

        if (entityToDelete == null)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = "Entity to delete not found.";
            return response;
        }

        Set.Remove(entityToDelete);

        try
        {
            await Context.SaveChangesAsync();
            response.Entity = Mapper.Map<TModel>(entityToDelete);
        }
        catch (Exception e)
        {
            Logger.LogError("Error on database {Exception}", e.ToString());
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Message = "Internal server error.";
        }

        return response;
    }

    /// <inheritdoc/>
    public virtual async Task<ProcessingStatusResponse<TModel>> FindByIdAsync(long id)
    {
        var response = new ProcessingStatusResponse<TModel>();
        try
        {
            var entity = await Set.FindAsync(id);

            if (entity == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Entity not found.";
                return response;
            }

            response.Entity = Mapper.Map<TModel>(entity);
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