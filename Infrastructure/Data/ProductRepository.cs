namespace Infrastructure.Data;
public class ProductRepository : IProductRepository
{
    private readonly StoreContext _storeContext;

    public ProductRepository(StoreContext storeContext)
    {
        _storeContext = Guard.Against.Null(storeContext, nameof(storeContext));
    }

    public async Task<IReadOnlyList<ProductBrand>> GetAllProductsBrandsAsync()
    {
        return await _storeContext.ProductBrands
                                  .AsNoTracking()
                                  .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetAllProductsTypesAsync()
    {
        return await _storeContext.ProductTypes
                                  .AsNoTracking()
                                  .ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _storeContext.Products
             .Include(product => product.ProductBrand)
             .Include(product => product.ProductType)
             .SingleOrDefaultAsync(product => product.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _storeContext.Products
            .Include(product => product.ProductBrand)
            .Include(product => product.ProductType)
            .AsNoTracking()
            .ToListAsync();
    }
}

