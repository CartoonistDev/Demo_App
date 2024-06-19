using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Auth.API.Services.Repository;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;

    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = _context.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        if (user != null)
        {
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                //Create role if role doesn't exist
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

            }

            await _userManager.AddToRoleAsync(user, roleName);

            return true;
        }

        return false;

    }

    public async Task<LoginResponseDto> Login(LoginRequestDto model)
    {
        var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == model.Username.ToLower());

        bool isValid = await _userManager.CheckPasswordAsync(user, model.Password);

        if (user == null || isValid == false)
        {
            return new LoginResponseDto()
            {
                UserDto = null,
                Token = string.Empty
            };
        }

        //Get User role

        var role = await _userManager.GetRolesAsync(user);

        //If user was found we generate Jwt Token

        var token = _jwtTokenGenerator.GenerateToken(user, role);

        UserDto userDto = new()
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            Name = user.UserName,
            PhoneNumber = user.PhoneNumber,
            
        };

        LoginResponseDto responseDto = new()
        {
            UserDto = userDto,
            Token = token
        };

        return responseDto;
    }

    public async Task<UserCreationResponse> Register(RegistrationRequestDto registrationRequestDto)
    {
        var response = new UserCreationResponse();
        var data = new UserCreationResponseDto();

        ApplicationUser newUser = new()
        {
            UserName = registrationRequestDto.UserName,
            Firstname = registrationRequestDto.Firstname,
            Surname = registrationRequestDto.Surname,
            Middlename = registrationRequestDto.Middlename,
            Email = registrationRequestDto.Email,
            EmailAddress = registrationRequestDto.Email,
            NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            NormalizedUserName = registrationRequestDto.UserName,
            PhoneNumber = registrationRequestDto.PhoneNumber,
        };

        try
        {
            //Register Functionality => Creating a User

            newUser.PasswordHash = _userManager.PasswordHasher.HashPassword(newUser, registrationRequestDto.Password);


            var result = await _userManager.CreateAsync(newUser, registrationRequestDto.Password);

            if (!result.Succeeded)
            {
                response.isSuccess = false;
                response.message = $"New User creation failed. [{result.Errors.FirstOrDefault().Description}]"; ;
                return response;
            }

            // Assign role to user
            if (!string.IsNullOrEmpty(registrationRequestDto.RoleName)) // Assuming role name is provided in input
            {
                var roleResult = await AssignRole(registrationRequestDto.Email, registrationRequestDto.RoleName);
                if (!roleResult)
                {
                    var delteUser = await _userManager.DeleteAsync(newUser);

                    response.isSuccess = false;
                    response.message = $"New User creation failed. [{result.Errors.FirstOrDefault().Description}]"; ;
                    return response;
                }
            }

            data = new UserCreationResponseDto
            {
                UserId = newUser.UserId,
                Firstname = newUser.Firstname,
                Middlename = newUser.Middlename,
                Surname = newUser.Surname,
                EmailAddress = newUser.EmailAddress,
                PhoneNumber = newUser.PhoneNumber,
                CreatedOn = newUser.CreatedOn,
                IsActive = newUser.IsActive,
                Status = UserStatus.Approved.ToString(),
            };

            response.isSuccess = true;
            response.message = $"New User creation Successful.";
            response.data = data;  
        }
        catch (Exception ex)
        {

        }

        return response;

    }

    public async Task<bool> CreateUserRole(RoleCreationDto roles)
    {
        try
        {
            if(string.IsNullOrEmpty(roles.Name))
            {
                return false;
            }
            var role = await _roleManager.CreateAsync(new IdentityRole(roles.Name));

            if (role.Succeeded)
            {
                return true ;
            }
        }
        catch (Exception ex)
        {

        }
        return false;
    }

    public async Task<UserCreationResponse> GetUserDetailsByEmail(string email)
    {
        var response = new UserCreationResponse();
        var data = new UserCreationResponseDto();

        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return response;
            }

            var userDetails = await _userManager.FindByEmailAsync(email);

            data = new UserCreationResponseDto
            {
                UserId = userDetails.UserId,
                Firstname = userDetails.Firstname,
                Middlename = userDetails.Middlename,
                Surname = userDetails.Surname,
                EmailAddress = userDetails.EmailAddress,
                PhoneNumber = userDetails.PhoneNumber,
                CreatedOn = userDetails.CreatedOn,
                IsActive = userDetails.IsActive,
                Status = UserStatus.Approved.ToString(),
            };

            response.isSuccess = true;
            response.message = $"User retrieved successfully.";
            response.data = data;

        }
        catch (Exception ex)
        {
        }
        return response;

    }
}
