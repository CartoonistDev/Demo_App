namespace Auth.API.Model.Dto;

public class TokenReturnDTO
{
    public string AccessToken { get; set; }
    public DateTime? ExpiresIn { get; set; }
}
