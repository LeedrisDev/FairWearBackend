using AutoMapper;
using BrandAndProductDatabase.Service.DataAccess.IRepositories;
using BrandAndProductDatabase.Service.Models.Dto;
using BrandAndProductDatabase.Service.Models.Entity;

namespace BrandAndProductDatabase.Service.DataAccess.Repositories;

/// <summary>Class representing a repository for the ProductEntity.</summary>
public class ProductRepository : Repository<ProductDto, ProductEntity>, IProductRepository
{
    /// <summary>Constructor for ProductRepository</summary>
    public ProductRepository(BrandAndProductDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}