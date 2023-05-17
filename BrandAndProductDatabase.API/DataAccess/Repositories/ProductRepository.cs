using AutoMapper;
using BrandAndProductDatabase.API.DataAccess.IRepositories;

namespace BrandAndProductDatabase.API.DataAccess.Repositories;

/// <summary>Class representing a repository for the ProductEntity.</summary>
public class ProductRepository : Repository<Models.Dto.ProductDto, Models.Entity.ProductEntity>, IProductRepository
{
    /// <summary>Constructor for ProductRepository</summary>
    public ProductRepository(BrandAndProductDbContext context, IMapper mapper) : base(context, mapper) { }
}