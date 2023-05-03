using BrandAndProductDatabase.API.DataAccess.IRepositories;

namespace BrandAndProductDatabase.API.DataAccess.Repositories;

/// <summary>Class representing a generic repository.</summary>
public class Repository<TModel, TEntity> : IRepository<TModel>
    where TModel : class
    where TEntity : class
{
    
}