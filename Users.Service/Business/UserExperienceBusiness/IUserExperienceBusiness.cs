using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.UserExperienceBusiness
{
    /// <summary>
    /// Interface for UserExperienceBusiness.
    /// </summary>
    public interface IUserExperienceBusiness
    {
        /// <summary>Gets all the UserExperiences.</summary>
        /// <returns>A list of <see cref="UserExperienceDto"/>.</returns>
        Task<ProcessingStatusResponse<IEnumerable<UserExperienceDto>>> GetAllUserExperiencesAsync(
            Dictionary<string, string> filters);

        /// <summary>Gets a UserExperience by Id.</summary>
        /// <param name="id">The Id of the UserExperience.</param>
        /// <returns>A <see cref="UserExperienceDto"/>.</returns>
        Task<ProcessingStatusResponse<UserExperienceDto>> GetUserExperienceByIdAsync(int id);

        /// <summary>Creates a UserExperience.</summary>
        /// <param name="userExperienceDto">The UserExperience to create.</param>
        /// <returns>A <see cref="UserExperienceDto"/>.</returns>
        Task<ProcessingStatusResponse<UserExperienceDto>>
            CreateUserExperienceAsync(UserExperienceDto userExperienceDto);

        /// <summary>Updates a UserExperience.</summary>
        /// <param name="userExperienceDto">The UserExperience to update.</param>
        /// <returns>A <see cref="UserExperienceDto"/>.</returns>
        Task<ProcessingStatusResponse<UserExperienceDto>>
            UpdateUserExperienceAsync(UserExperienceDto userExperienceDto);

        /// <summary>Deletes a UserExperience.</summary>
        /// <param name="id">The Id of the UserExperience to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task<ProcessingStatusResponse<UserExperienceDto>> DeleteUserExperienceAsync(int id);
    }
}