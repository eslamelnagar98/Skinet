namespace API.Helpers;
public class MappingUrlResolver<Tsource, TDestination> : IValueResolver<Tsource, TDestination, string>
    where TDestination : class
    where Tsource : class

{
    private readonly IConfiguration _configuration;
    public MappingUrlResolver(IConfiguration configuration)
    {
        _configuration = Guard.Against.Null(configuration, nameof(configuration));
    }
    public string Resolve(Tsource source, TDestination destination, string destMember, ResolutionContext context)
    {
        return Guard.Against.NullOrEmptyObject(source, _configuration);
    }
}
