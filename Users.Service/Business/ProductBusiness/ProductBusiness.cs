using Users.Service.DataAccess.Filters;
using Users.Service.DataAccess.IRepositories;
using Users.Service.Models;
using Users.Service.Models.Dto;

namespace Users.Service.Business.ProductBusiness
{
    /// <summary>Business logic for products database actions </summary>
    public class ProductBusiness : IProductBusiness
    {
        private readonly IFilterFactory<IFilter> _filterFactory;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Constructor for ProductBusiness.
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="filterFactory"></param>
        public ProductBusiness(IProductRepository productRepository, IFilterFactory<IFilter> filterFactory)
        {
            _productRepository = productRepository;
            _filterFactory = filterFactory;
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<IEnumerable<ProductDto>>> GetAllProductsAsync(
            Dictionary<string, string> filters)
        {
            var filter = _filterFactory.CreateFilter(filters);

            return await _productRepository.GetAllAsync(filter);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<ProductDto>> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<ProductDto>> CreateProductAsync(ProductDto productDto)
        {
            return await _productRepository.AddAsync(productDto);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<ProductDto>> UpdateProductAsync(ProductDto productDto)
        {
            return await _productRepository.UpdateAsync(productDto);
        }

        /// <inheritdoc/>
        public async Task<ProcessingStatusResponse<ProductDto>> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}