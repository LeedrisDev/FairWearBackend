using System.Net;
using AutoMapper;
using BrandAndProduct.Service.DataAccess.IRepositories;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Models.Entity;

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
            brandEntity = Context.Brands.FirstOrDefault(x => x.Id == entity.BrandId);
        }


        Mapper.Map(entity, entityToUpdate);
        if (brandEntity != null) entityToUpdate.BrandEntity = brandEntity;

        DbSet.Update(entityToUpdate);
        await Context.SaveChangesAsync();

        processingStatusResponse.Object = Mapper.Map<ProductDto>(entityToUpdate);
        return processingStatusResponse;
    }
}