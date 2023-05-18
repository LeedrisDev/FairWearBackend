using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProductDatabase.API.DataAccess.Repositories;

/// <summary>Class representing a repository for the ProductEntity.</summary>
public class ProductRepository : Repository<ProductDto, ProductEntity>, IProductRepository
{
    /// <summary>Constructor for ProductRepository</summary>
    public ProductRepository(BrandAndProductDbContext context, IMapper mapper) : base(context, mapper) { }
    
    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<ProductDto>> GetProductByBarcodeAsync(string barcode)
    {
        var processingStatusResponse = new ProcessingStatusResponse<ProductDto>();
        
        var entity = await Context.Products.FirstOrDefaultAsync(p => p.UpcCode == barcode);
        
        if (entity == null)
        {
            processingStatusResponse.Status = HttpStatusCode.NotFound;
            processingStatusResponse.ErrorMessage = $"Entity with barcode {barcode} not found.";
            return processingStatusResponse;
        }
        
        processingStatusResponse.Object = Mapper.Map<ProductDto>(entity);
        return processingStatusResponse;
    }
}