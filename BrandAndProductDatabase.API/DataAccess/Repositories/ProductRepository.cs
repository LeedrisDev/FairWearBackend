using AutoMapper;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Models.Entity;

namespace BrandAndProductDatabase.API.DataAccess.Repositories;

/// <summary>Class representing a repository for the ProductEntity.</summary>
public class ProductRepository : Repository<ProductDto, ProductEntity>, IProductRepository
{
    /// <summary>Constructor for ProductRepository</summary>
    public ProductRepository(BrandAndProductDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}