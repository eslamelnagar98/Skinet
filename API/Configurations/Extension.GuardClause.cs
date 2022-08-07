namespace API.Configurations;
public partial class Extension
{
    public static string NullOrEmptyProduct(this IGuardClause guard,Product product , IConfiguration configuration)
    {
        if(string.IsNullOrEmpty(product.PictureUrl))
            return null;
        var apiUrl= configuration.GetValue<string>("ApiUrl");
        return $"{apiUrl}{product.PictureUrl}"; 
    }
}

