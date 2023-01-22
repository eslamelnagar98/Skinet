namespace Infrastructure.Identity;
public class AppIdentityDbContextSeed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        var isUserManagerHasData = userManager?.Users?.Any() ?? true;
        if (isUserManagerHasData) return;
        var user = new AppUser
        {
            DisplayName = "Islam Elnagar",
            Email = "IslamElnagar@gmail.com",
            UserName = "IslamElnagar@gmail.com",
            Address = new Address
            {
                FirstName = "Islam",
                LastName = "Elnagar",
                Street = "Volgrad",
                City = "Portsaid",
                State = "Portsaid",
                ZipCode = "90210"
            }
        };
         await userManager.CreateAsync(user, "Pa$$w0rd");
    }
}
