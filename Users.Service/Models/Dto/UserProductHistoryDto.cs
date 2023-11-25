namespace Users.Service.Models.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for user's product history information.
    /// </summary>
    public class UserProductHistoryDto : IObjectWithId
    {
        /// <summary>
        /// Gets or sets the ID of the user associated with this product history.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the product associated with this product history.
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the product history (if available).
        /// </summary>
        public DateOnly? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the entity representing the product associated with this history.
        /// </summary>
        public virtual ProductDto ProductEntity { get; set; } = null!;

        /// <summary>
        /// Gets or sets the entity representing the user associated with this product history.
        /// </summary>
        public virtual UserDto UserEntity { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the user's product history.
        /// </summary>
        public long Id { get; set; }
    }
}