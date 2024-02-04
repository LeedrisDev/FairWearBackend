using AutoMapper;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models.Dto;
using Users.Service.Models.Entity;

namespace Users.Service.DataAccess.Repositories
{
    /// <summary>
    /// Repository handling operations related to User Product History entities and DTOs.
    /// </summary>
    public class UserProductHistoryRepository : Repository<UserProductHistoryDto, UserProductHistoryEntity>,
        IUserProductHistoryRepository
    {
        /// <summary>
        /// Constructor for the UserProductHistoryRepository.
        /// </summary>
        /// <param name="context">The UsersDbContext instance.</param>
        /// <param name="mapper">The AutoMapper instance for entity-DTO mapping.</param>
        public UserProductHistoryRepository(UsersDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}