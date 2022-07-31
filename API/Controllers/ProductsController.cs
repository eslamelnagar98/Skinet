using Core.Specifications;

namespace API.Controllers;
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<ProductBrand> _productBrand;
    private readonly IGenericRepository<ProductType> _productType;

    public ProductsController(
        IGenericRepository<Product> productRepository,
        IGenericRepository<ProductBrand> productBrand,
        IGenericRepository<ProductType> productType)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(productRepository));
        _productBrand = Guard.Against.Null(productBrand, nameof(productBrand));
        _productType = Guard.Against.Null(productType, nameof(productType));
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var specification = new ProductsWithTypesAndBrandsSpecification();
        var products = await _productRepository.ListAsync(specification);
        return Ok(products);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<List<Product>>> GetProductsBrands()
    {
        var products = await _productBrand.ListAllAsync();
        return Ok(products);
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<Product>>> GetProductsTypes()
    {
        var products = await _productType.ListAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var specification = new ProductsWithTypesAndBrandsSpecification(id);
        var product = await _productRepository.GetEntityWithSpecification(specification);
        return Ok(product);
    }
}

