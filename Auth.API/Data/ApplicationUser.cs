using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Auth.API.Data;

public class ApplicationUser : IdentityUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UserId { get; set; }

    [Required]
    [StringLength(maximumLength: 250)]
    public string Firstname { get; set; }
    public string Middlename { get; set; }

    [Required]
    [StringLength(maximumLength: 250)]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }

    [Required]
    [Phone]
    public override string PhoneNumber { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    [StringLength(250)]
    public string? ModifiedBy { get; set; } = null;

    public DateTime? ModifiedOn { get; set; }

    public bool IsActive { get; set; } = false;

    public DateTime LastLogin { get; set; } = DateTime.Now;
}
