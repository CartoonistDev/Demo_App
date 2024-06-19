namespace Auth.API.Services.IRepository;

public interface IAuthService
{
    Task<UserCreationResponse> Register(RegistrationRequestDto registrationRequestDto);
    Task<LoginResponseDto> Login(LoginRequestDto requestDto);
    Task<bool> CreateUserRole(RoleCreationDto roles);
    Task<bool> AssignRole(string email, string roleName);
    Task<UserCreationResponse> GetUserDetailsByEmail(string email);
}
