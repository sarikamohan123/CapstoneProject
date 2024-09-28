using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace PrsWeb.Models;

[Table("Product")]
[Index("VendorId", "PartNumber", Name = "UQ_product_Vid_Pnum", IsUnique = true)]
public partial class Product
{
    [Key]
    public int Id { get; set; }

    public int VendorId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PartNumber { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Unit { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Photopath { get; set; }

    [InverseProperty("Product")]
    [JsonIgnore]
    public virtual ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();

    [ForeignKey("VendorId")]
    [InverseProperty("Products")]
    public virtual Vendor? Vendor { get; set; } = null!;

    //[JsonIgnore]
    //[InverseProperty("Product")]
    //public virtual ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();

}
