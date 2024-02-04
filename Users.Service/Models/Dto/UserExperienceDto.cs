namespace Users.Service.Models.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for user experience information.
    /// </summary>
    public class UserExperienceDto : IObjectWithId
    {
        /// <summary>
        /// Gets or sets the ID of the user associated with this experience.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the score related to this user experience (if available).
        /// </summary>
        public long? Score { get; set; }

        /// <summary>
        /// Gets or sets the level of the user's experience (if available).
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// Gets or sets the array of to-do items related to this user experience (if available).
        /// </summary>
        public int[]? Todos { get; set; }

        /// <summary>
        /// Gets or sets the entity representing the user associated with this experience.
        /// </summary>
        public virtual UserDto UserEntity { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the user experience.
        /// </summary>
        public long Id { get; set; }
    }
}