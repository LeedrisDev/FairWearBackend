using AutoMapper;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models.Dto;
using Users.Service.Models.Entity;

namespace Users.Service.DataAccess.Repositories
{
    /// <summary>
    /// Repository handling operations related to Product entities and DTOs.
    /// </summary>
    public class ProductRepository : Repository<ProductDto, ProductEntity>, IProductRepository
    {
        /// <summary>
        /// Constructor for the ProductRepository.
        /// </summary>
        /// <param name="context">The UsersDbContext instance.</param>
        /// <param name="mapper">The AutoMapper instance for entity-DTO mapping.</param>
        public ProductRepository(UsersDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}