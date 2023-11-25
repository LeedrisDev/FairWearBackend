using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.UserBusiness
{
    /// <summary>
    /// Interface for UserBusiness.
    /// </summary>
    public interface IUserBusiness
    {
        /// <summary>Gets all the Users.</summary>
        /// <returns>A list of <see cref="UserDto"/>.</returns>
        Task<ProcessingStatusResponse<IEnumerable<UserDto>>> GetAllUsersAsync(Dictionary<string, string> filters);

        /// <summary>Gets a User by Id.</summary>
        /// <param name="id">The Id of the User.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        Task<ProcessingStatusResponse<UserDto>> GetUserByIdAsync(int id);

        /// <summary>Creates a User.</summary>
        /// <param name="userDto">The User to create.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        Task<ProcessingStatusResponse<UserDto>> CreateUserAsync(UserDto userDto);

        /// <summary>Updates a User.</summary>
        /// <param name="userDto">The User to update.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        Task<ProcessingStatusResponse<UserDto>> UpdateUserAsync(UserDto userDto);

        /// <summary>Deletes a User.</summary>
        /// <param name="id">The Id of the User to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task<ProcessingStatusResponse<UserDto>> DeleteUserAsync(int id);
    }
}