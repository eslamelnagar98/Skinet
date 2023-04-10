namespace API.Helpers;
public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
{
    private readonly IConfiguration _configuration;

    public ProductUrlResolver(IConfiguration configuration)
    {
        _configuration = Guard.Against.Null(configuration,nameof(configuration));
    }
    public string Resolve(Product product, ProductToReturnDto productToReturnDto, string destMember, ResolutionContext context)
    {
        return Guard.Against.NullOrEmptyObject(product, _configuration);
    }
}

