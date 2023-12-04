using Users.Service.Models.Dto;

namespace Users.Service.DataAccess.IRepositories
{
    /// <summary>
    /// Interface representing a repository for Product DTOs.
    /// </summary>
    public interface IProductRepository : IRepository<ProductDto>
    {
    }
}