namespace Users.Service.Models.Entity
{
    /// <summary>
    /// Represents a User entity.
    /// </summary>
    public partial class UserEntity : IObjectWithId
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string Phone { get; set; } = null!;

        /// <summary>
        /// Gets or sets the language preferences of the user (if any).
        /// </summary>
        public string? LanguagePreferences { get; set; }

        /// <summary>
        /// Gets or sets the theme preference of the user (if any).
        /// </summary>
        public string? Theme { get; set; }

        /// <summary>
        /// Gets or sets the Firebase ID associated with the user.
        /// </summary>
        public string FirebaseId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of user experiences associated with this user.
        /// </summary>
        public virtual ICollection<UserExperienceEntity> UserExperiences { get; set; } =
            new List<UserExperienceEntity>();

        /// <summary>
        /// Gets or sets the collection of user product histories associated with this user.
        /// </summary>
        public virtual ICollection<UserProductHistoryEntity> UserProductHistories { get; set; } =
            new List<UserProductHistoryEntity>();

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public long Id { get; set; }
    }
}