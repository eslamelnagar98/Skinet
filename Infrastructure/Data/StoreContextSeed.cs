namespace Infrastructure.Data;
public class StoreContextSeed
{
    private static StoreContext _storeContext;
    public static async Task SeedAsync(StoreContext storeContext, ILogger logger)
    {
        _storeContext = storeContext;
        try
        {
            var productSeedList = await GetAllProductSeedDtoList();

            foreach (var productSeet in productSeedList)
            {
                if (productSeet?.IsEmpty ?? false)
                {
                    await productSeet?.ProductSeedMethod?.Invoke();
                }
            }
            await _storeContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError("Something Wrong Happen , {exp}", exception);
            throw;
        }
    }

    private static async Task AddskinetSeedData<TEntity>(string entityFilePath)
    {
        var basePath = "../Infrastructure/Data/SeedData/";
        var entityPath = $@"{basePath}/{entityFilePath}";
        var entityData = await File.ReadAllTextAsync(entityPath);
        var entityRows = JsonSerializer.Deserialize<List<TEntity>>(entityData);
        entityRows.ForEach(async entityRow => await _storeContext.AddAsync(entityRow));

    }

    private static async Task<IReadOnlyList<ProductSeedDto>> GetAllProductSeedDtoList()
    {
        return new List<ProductSeedDto>
            {
             new ProductSeedDto
                {
                    IsEmpty=await _storeContext.Products.CountAsync() == 0,
                    ProductSeedMethod=async ()=> await AddskinetSeedData<Product>("products.json")
                },
                new ProductSeedDto
                {
                    IsEmpty=await _storeContext.ProductBrands.CountAsync() == 0,
                    ProductSeedMethod=async ()=> await AddskinetSeedData<ProductBrand>("brands.json")
                },
                new ProductSeedDto
                {
                    IsEmpty=await _storeContext.ProductTypes.CountAsync() == 0,
                    ProductSeedMethod=async ()=> await AddskinetSeedData<ProductType>("types.json")
                }
            };
    }
}

