namespace API.Controllers;
public class ProductsController : BaseApiController
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<ProductBrand> _productBrand;
    private readonly IGenericRepository<ProductType> _productType;
    private readonly IMapper _mapper;

    public ProductsController(IGenericRepository<Product> productRepository,
                              IGenericRepository<ProductBrand> productBrand,
                              IGenericRepository<ProductType> productType,
                              IMapper mapper)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(productRepository));
        _productBrand = Guard.Against.Null(productBrand, nameof(productBrand));
        _productType = Guard.Against.Null(productType, nameof(productType));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecificationParams productParams)
    {
        var specification = new ProductsWithTypesAndBrandsSpecification(productParams);
        var countSpecification = new ProductWithFilterForCountSpecification(productParams);
        var totalItems = await _productRepository.CountAsync(countSpecification);
        var products = await _productRepository.ListAsync(specification);
        var productsToReturn = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
        var paginationToReturn = GeneratePagination(productParams, totalItems, productsToReturn);
        return Ok(paginationToReturn);
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
    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var specification = new ProductsWithTypesAndBrandsSpecification(id);
        var product = await _productRepository.GetEntityWithSpecification(specification);
        if (product is null)
        {
            return NotFound(new ApiResponse(404));
        }
        var productToReturn = _mapper.Map<ProductToReturnDto>(product);
        return Ok(productToReturn);
    }

    private Pagination<ProductToReturnDto> GeneratePagination(ProductSpecificationParams productParams,
                                                              int totalItems,
                                                              IReadOnlyList<ProductToReturnDto> data)
    {
        var listCount = data?.Count ?? 0;
        return new Pagination<ProductToReturnDto>
        {
            PageIndex = productParams.PageIndex,
            PageSize = productParams.PageSize <= listCount ? productParams.PageSize : listCount,
            Count = totalItems,
            Data = data
        };
    }
}

