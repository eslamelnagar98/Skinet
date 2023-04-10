namespace API.Configurations;
public partial class Extension
{
    public static string NullOrEmptyObject<TEntity>(this IGuardClause guard, TEntity entity, IConfiguration configuration)
        where TEntity : class
    {
        dynamic dynamicEntity = entity;
        if (string.IsNullOrEmpty(dynamicEntity.PictureUrl))
            return null;
        var apiUrl = configuration.GetValue<string>("ApiUrl");
        return $"{apiUrl}{dynamicEntity.PictureUrl}";
    }
}

