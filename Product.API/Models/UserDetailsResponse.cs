namespace Product.API.Models;

public class UserDetailsResponseDto
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

public class UserDetailsResponse
{
    public bool isSuccess { get; set; }
    public string message { get; set; }

    public UserDetailsResponseDto data { get; set; }
}

public class UserProducts
{
    public string UserEmail { get; set; }
    public string PhoneNumber { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription {  get; set; }
}