namespace Auth.API.Controller;
[Route("api/user")]
[ApiController]
[Authorize(Roles = "Customer, Administrator")]
public class UserController : ControllerBase
{

    private readonly IAuthService _authService;
    protected ResponseDto _responseDto;

    public UserController(IAuthService authService)
    {
        _authService = authService;
        _responseDto = new();
    }


    [HttpPost("getUserByEmail")]
    public async Task<IActionResult> GetUserByEmail([FromBody] string email)
    {
        var auth = await _authService.GetUserDetailsByEmail(email);
        if (auth.isSuccess == false)
        {
            return BadRequest(auth);
        }
        return Ok(auth);
    }

}