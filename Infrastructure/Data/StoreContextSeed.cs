using Infrastructure.Data.DTOs;

namespace Infrastructure.Data;
public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext storeContext, ILogger logger)
    {

        try
        {
            var productSeedList = new List<ProductSeedDto>
            {
                new ProductSeedDto
                {
                    IsEmpty=await storeContext.ProductBrands.CountAsync() == 0,
                    ProductSeedMethod=async ()=> await AddskinetSeedData<ProductBrand>(storeContext, "brands.json")
                },
                new ProductSeedDto
                {
                    IsEmpty=await storeContext.ProductTypes.CountAsync() == 0,
                    ProductSeedMethod=async ()=> await AddskinetSeedData<ProductType>(storeContext, "types.json")
                },
                new ProductSeedDto
                {
                    IsEmpty=await storeContext.Products.CountAsync() == 0,
                    ProductSeedMethod=async ()=> await AddskinetSeedData<Product>(storeContext, "products.json")
                },
            };

            foreach (var productSeet in productSeedList)
            {
                if (productSeet?.IsEmpty ?? false)
                {
                    await productSeet?.ProductSeedMethod?.Invoke();
                }
            }
            //var productBrandsIsEmpty = await storeContext.ProductBrands.CountAsync() == 0;
            //var productTypeIsEmpty = await storeContext.ProductTypes.CountAsync() == 0;
            //var productsIsEmpty = await storeContext.Products.CountAsync() == 0;
            //var productBrandfilePath = "brands.json";
            //var productTypefilePath = "types.json";
            //var productsfilePath = "products.json";

            //var tablesIsEmptyDictionary = new Dictionary<string, bool>
            //{
            //    {"productBrandsIsEmpty",productBrandsIsEmpty },
            //    {"productTypeIsEmpty",productTypeIsEmpty },
            //    {"productsIsEmpty",productsIsEmpty },
            //};


            //var productSeetDictionary = new Dictionary<string, Func<Task>>
            //{
            //    { "productBrandsIsEmpty",async()=>await AddskinetSeedData<ProductBrand>(storeContext, productBrandfilePath) },
            //    { "productTypeIsEmpty",async()=>await AddskinetSeedData<ProductType>(storeContext, productTypefilePath) },
            //    { "productsIsEmpty",async()=>await AddskinetSeedData<Product>(storeContext, productsfilePath) },
            //};

            //foreach (var productSeet in productSeetDictionary)
            //{
            //    tablesIsEmptyDictionary.TryGetValue(productSeet.Key, out var isEmpty);
            //    if (isEmpty)
            //    {
            //        await productSeet.Value?.Invoke();
            //    }
            //}




            await storeContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError("Something Wrong Happen , {exp}", exception);

        }
    }

    private static async Task AddskinetSeedData<TEntity>(StoreContext storeContext, string entityFilePath)
    {
        var basePath = "../Infrastructure/Data/SeedData/";
        var entityPath = $@"{basePath}/{entityFilePath}";
        var entityData = await File.ReadAllTextAsync(entityPath);
        var entityRows = JsonSerializer.Deserialize<List<TEntity>>(entityData);
        entityRows.ForEach(async entityRow => await storeContext.AddAsync(entityRow));
       
    }
}

