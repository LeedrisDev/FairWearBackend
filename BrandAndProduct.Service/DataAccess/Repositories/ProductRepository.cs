using System.Net;
using AutoMapper;
using BrandAndProduct.Service.DataAccess.IRepositories;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProduct.Service.DataAccess.Repositories;

/// <summary>Class representing a repository for the ProductEntity.</summary>
public class ProductRepository : Repository<ProductDto, ProductEntity>, IProductRepository
{
    /// <summary>Constructor for ProductRepository</summary>
    public ProductRepository(BrandAndProductDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> UpdateProductAsync(ProductDto entity)
    {
        var processingStatusResponse = new ProcessingStatusResponse<ProductDto>();
        var entityToUpdate = await DbSet.FindAsync(entity.Id);

        if (entityToUpdate == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Entity with ID {entity.Id} not found.";
            return processingStatusResponse;
        }

        BrandEntity brandEntity = entityToUpdate.BrandEntity;

        if (entityToUpdate.BrandId != entity.BrandId)
        {
            brandEntity = Context.Brands.FirstOrDefault(x => x.Id == entity.BrandId)!;
        }


        Mapper.Map(entity, entityToUpdate);
        entityToUpdate.BrandEntity = brandEntity;

        DbSet.Update(entityToUpdate);
        await Context.SaveChangesAsync();

        processingStatusResponse.Object = Mapper.Map<ProductDto>(entityToUpdate);
        return processingStatusResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<ProductDto>>> GetRecommendedProductsAsync(int productId)
    {
        var processingStatusResponse = new ProcessingStatusResponse<IEnumerable<ProductDto>>();
        var productEntity = await DbSet.FindAsync(productId);

        if (productEntity == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Entity with ID {productId} not found.";
            return processingStatusResponse;
        }

        var recommendedProducts = await DbSet
            .AsNoTracking()
            .Where(e => e.Category == productEntity.Category && e.Id != productId)
            .OrderBy(e => e.Color == productEntity.Color)
            .Take(20)
            .ToListAsync();

        processingStatusResponse.Object = Mapper.Map<IEnumerable<ProductDto>>(recommendedProducts);
        return processingStatusResponse;
    }
}