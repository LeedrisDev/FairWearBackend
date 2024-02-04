using AutoMapper;
using BrandAndProduct.Service.DataAccess.IRepositories;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Models.Entity;

namespace BrandAndProduct.Service.DataAccess.Repositories;

/// <summary>Class representing a repository for the IntegrationEventEntity.</summary>
public class IntegrationEventRepository : Repository<IntegrationEventDto, IntegrationEventEntity>,
    IIntegrationEventRepository
{
    /// <summary>Constructor</summary>
    public IntegrationEventRepository(BrandAndProductDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}