using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.UserProductHistoryBusiness
{
    /// <summary>
    /// Interface for UserProductHistoryBusiness.
    /// </summary>
    public interface IUserProductHistoryBusiness
    {
        /// <summary>Gets all the UserProductHistorys.</summary>
        /// <returns>A list of <see cref="UserProductHistoryDto"/>.</returns>
        Task<ProcessingStatusResponse<IEnumerable<UserProductHistoryDto>>> GetAllUserProductHistorysAsync(
            Dictionary<string, string> filters);

        /// <summary>Gets a UserProductHistory by Id.</summary>
        /// <param name="id">The Id of the UserProductHistory.</param>
        /// <returns>A <see cref="UserProductHistoryDto"/>.</returns>
        Task<ProcessingStatusResponse<UserProductHistoryDto>> GetUserProductHistoryByIdAsync(long id);

        /// <summary>Creates a UserProductHistory.</summary>
        /// <param name="userProductHistoryDto">The UserProductHistory to create.</param>
        /// <returns>A <see cref="UserProductHistoryDto"/>.</returns>
        Task<ProcessingStatusResponse<UserProductHistoryDto>> CreateUserProductHistoryAsync(
            UserProductHistoryDto userProductHistoryDto);

        /// <summary>Updates a UserProductHistory.</summary>
        /// <param name="userProductHistoryDto">The UserProductHistory to update.</param>
        /// <returns>A <see cref="UserProductHistoryDto"/>.</returns>
        Task<ProcessingStatusResponse<UserProductHistoryDto>> UpdateUserProductHistoryAsync(
            UserProductHistoryDto userProductHistoryDto);

        /// <summary>Deletes a UserProductHistory.</summary>
        /// <param name="id">The Id of the UserProductHistory to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task<ProcessingStatusResponse<UserProductHistoryDto>> DeleteUserProductHistoryAsync(long id);
    }
}