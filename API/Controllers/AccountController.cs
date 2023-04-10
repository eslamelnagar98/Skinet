namespace API.Controllers;
public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    public AccountController(UserManager<AppUser> userManager,
                             SignInManager<AppUser> signInManager,
                             ITokenService tokenService)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
        _signInManager = Guard.Against.Null(signInManager, nameof(signInManager));
        _tokenService = Guard.Against.Null(tokenService, nameof(tokenService));
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) is not null;
    }

    [Authorize, HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
        var userDto = UserDto.CreateInstance(user);
        userDto.Token = _tokenService.CreateToken(user);
        return userDto;
    }

    [Authorize, HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {
        var user = await _userManager.FindByEmailWithAddressAsync(User);

        return (AddressDto)user.Address;
    }

    [Authorize, HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
    {
        var user = await _userManager.FindByEmailWithAddressAsync(User);

        user.Address = (Address)address;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return Ok((AddressDto)user.Address);
        }

        return BadRequest("Problem updating the user");
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null)
        {
            return Unauthorized(new ApiResponse(401));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (result.Succeeded is false)
        {
            return Unauthorized(new ApiResponse(401));
        }
        var userDto = UserDto.CreateInstance(user);
        userDto.Token = _tokenService.CreateToken(user);
        return userDto;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var isUserEmailAlreadyExist = await CheckEmailExistsAsync(registerDto.Email);
        if (isUserEmailAlreadyExist.Value)
        {
            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = new() { "Email Address Already In Use" }
            });
        }
        var user = (AppUser)registerDto;
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (result.Succeeded is false)
        {
            return BadRequest(new ApiResponse(400));
        }

        var userDto = UserDto.CreateInstance(user);
        userDto.Token = _tokenService.CreateToken(user);
        return userDto;
    }
}
