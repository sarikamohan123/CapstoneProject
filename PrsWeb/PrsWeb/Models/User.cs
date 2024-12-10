﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PrsWeb.Models;

[Table("User")]
[Index("Email", Name = "uq_user_email", IsUnique = true)]
[Index("FirstName", "LastName", "PhoneNumber", Name = "uq_user_person", IsUnique = true)]
[Index("Username", Name = "uq_user_unique", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(12)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(75)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    public bool Reviewer { get; set; }

    public bool Admin { get; set; }
    [JsonIgnore]

    [InverseProperty("User")]
    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
