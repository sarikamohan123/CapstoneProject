using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PrsWeb.Models;

[Table("Vendor")]
[Index("Name", "Address", "City", "State", Name = "uq_Vendor_Business", IsUnique = true)]
[Index("Code", Name = "uq_vendor_code", IsUnique = true)]
public partial class Vendor
{
    [Key]
    public int Id { get; set; }

    [Column("code")]
    [StringLength(10)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [StringLength(2)]
    [Unicode(false)]
    public string State { get; set; } = null!;

    [StringLength(5)]
    [Unicode(false)]
    public string Zip { get; set; } = null!;

    [StringLength(12)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;
    [JsonIgnore]
    [InverseProperty("Vendor")]
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
