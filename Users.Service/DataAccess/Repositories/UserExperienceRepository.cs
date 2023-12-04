using AutoMapper;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models.Dto;
using Users.Service.Models.Entity;

namespace Users.Service.DataAccess.Repositories
{
    /// <summary>
    /// Repository handling operations related to User Experience entities and DTOs.
    /// </summary>
    public class UserExperienceRepository : Repository<UserExperienceDto, UserExperienceEntity>,
        IUserExperienceRepository
    {
        /// <summary>
        /// Constructor for the UserExperienceRepository.
        /// </summary>
        /// <param name="context">The UsersDbContext instance.</param>
        /// <param name="mapper">The AutoMapper instance for entity-DTO mapping.</param>
        public UserExperienceRepository(UsersDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}