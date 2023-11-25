using AutoMapper;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models.Dto;
using Users.Service.Models.Entity;

namespace Users.Service.DataAccess.Repositories
{
    /// <summary>
    /// Repository handling operations related to User entities and DTOs.
    /// </summary>
    public class UserRepository : Repository<UserDto, UserEntity>, IUserRepository
    {
        /// <summary>
        /// Constructor for the UserRepository.
        /// </summary>
        /// <param name="context">The UsersDbContext instance.</param>
        /// <param name="mapper">The AutoMapper instance for entity-DTO mapping.</param>
        public UserRepository(UsersDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}