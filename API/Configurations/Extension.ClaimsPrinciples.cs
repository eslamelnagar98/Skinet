namespace API.Configurations;
public partial class Extension
{
    public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Email);
    }
}
