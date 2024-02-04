using Users.Service.Models.Dto;

namespace Users.Service.DataAccess.IRepositories
{
    /// <summary>
    /// Interface representing a repository for User DTOs.
    /// </summary>
    public interface IUserRepository : IRepository<UserDto>
    {
    }
}