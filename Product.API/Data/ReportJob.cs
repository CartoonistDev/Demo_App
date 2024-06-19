namespace Product.API.Data;

public class ReportJob
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string ReportName { get; set; }
    public string ReportPath { get; set; }
    public bool Generated { get; set; }
}
