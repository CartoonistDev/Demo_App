namespace Auth.API.Controller;
[Route("api/auth")]
[ApiController]
public class AuthAPIController :  ControllerBase
{
    private readonly IAuthService _authService;
    protected ResponseDto _responseDto;

    public AuthAPIController(IAuthService authService)
    {
        _authService = authService;
        _responseDto = new();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto requestDto)
    {
        var auth = await _authService.Register(requestDto);
        if (auth.isSuccess == false)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = auth.message;

            return BadRequest(_responseDto);
        }
        return Ok(_responseDto);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
    {
        var loginResponse = await _authService.Login(requestDto);
        if (loginResponse.UserDto == null)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = "Username or password incorrect";

            return BadRequest(_responseDto);
        }
        _responseDto.Message = "Login Successful";
        _responseDto.Result = loginResponse;
        return Ok(_responseDto);
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost("createRole")]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreationDto requestDto)
    {
        var assignRoleResponse = await _authService.CreateUserRole(requestDto);
        if (!assignRoleResponse)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = "Role Creation Not Successful";

            return BadRequest(_responseDto);
        }
        _responseDto.Result = assignRoleResponse;
        return Ok(_responseDto);
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost("assignRole")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto requestDto)
    {
        var assignRoleResponse = await _authService.AssignRole(requestDto.Email, requestDto.RoleName.ToUpper());
        if (!assignRoleResponse)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = "Role Assign Not Successful";

            return BadRequest(_responseDto);
        }
        _responseDto.Result = assignRoleResponse;
        return Ok(_responseDto);
    }
}

