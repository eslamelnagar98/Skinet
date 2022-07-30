namespace API.Controllers;
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(productRepository));
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await _productRepository.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<List<Product>>> GetProductsBrands()
    {
        var products = await _productRepository.GetAllProductsBrandsAsync();
        return Ok(products);
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<Product>>> GetProductsTypes()
    {
        var products = await _productRepository.GetAllProductsTypesAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        return Ok(product);
    }
}

