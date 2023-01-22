namespace API.Dtos;
public class RegisterDto
{
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public static explicit operator AppUser(RegisterDto registerDto)
    {
        return new()
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };
    }
}
