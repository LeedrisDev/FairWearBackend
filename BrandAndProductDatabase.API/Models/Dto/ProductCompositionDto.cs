namespace BrandAndProductDatabase.API.Models.Dto
{
    /// <summary>
    /// Class representing a ProductComposition in the business.
    /// </summary>
    public class ProductCompositionDto
    {
        /// <summary>
        /// Percentage of the type of fabrics in the Product.
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// Type of fabric in the Product.
        /// </summary>
        public string Component { get; set; } = null!;
    }
}