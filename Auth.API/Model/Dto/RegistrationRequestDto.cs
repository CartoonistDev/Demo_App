namespace Auth.API.Model.Dto;

public class RegistrationRequestDto
{
    [Required]
    [StringLength(maximumLength: 250)]
    public string Firstname { get; set; }
    public string Middlename { get; set; }

    [Required]
    [StringLength(maximumLength: 250)]
    public string Surname { get; set; }
    [Required]
    public string UserName { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Phone]
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string RoleName { get; set; }
}

public class AssignRoleDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]
    public string RoleName { get; set; }
}
public class UserCreationResponseDto
{
    public long UserId { get; set; }
    public string Firstname { get; set; }
    public string Middlename { get; set; }
    public string Surname { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = false;
    public string Status { get; set; }
}

public class UserCreationResponse
{
    public bool isSuccess { get; set; }
    public string message { get; set; }

    public UserCreationResponseDto data { get; set; }
}
public class RoleCreationDto
{
    public string Name { get; set; } = Roles.Customer.ToString();
}