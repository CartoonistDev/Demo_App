namespace Product.API.Data;

public class EmailJob
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string EmailAddress { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool Sent { get; set; }
}

