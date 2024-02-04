namespace Users.Service.Models.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for Product information.
    /// </summary>
    public class ProductDto : IObjectWithId
    {
        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the rating of the product.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the collection of user product histories associated with this product.
        /// </summary>
        public virtual ICollection<UserProductHistoryDto> UserProductHistories { get; set; } =
            new List<UserProductHistoryDto>();

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public long Id { get; set; }
    }
}