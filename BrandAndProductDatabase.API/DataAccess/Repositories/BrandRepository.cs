using AutoMapper;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProductDatabase.API.DataAccess.Repositories;

/// <summary>Class representing a repository for the BrandEntity.</summary>
public class BrandRepository : Repository<Models.Dto.BrandDto, Models.Entity.BrandEntity>, IBrandRepository
{
    /// <summary>Constructor for BrandRepository</summary>
    public BrandRepository(DbContext context, IMapper mapper) : base(context, mapper) { }
}