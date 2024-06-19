global using System.ComponentModel.DataAnnotations.Schema;
global using System.ComponentModel.DataAnnotations;


namespace Product.API.Data;

[Table(nameof(OrderProduct))]

public class OrderProduct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<string> Category { get; set; } = new();
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; } = default;

}
