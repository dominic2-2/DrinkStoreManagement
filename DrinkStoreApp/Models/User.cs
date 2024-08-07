using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? DisplayName { get; set; }

    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public byte Status { get; set; }

    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Import> ImportApprovedByNavigations { get; set; } = new List<Import>();

    public virtual ICollection<Import> ImportCreatedByNavigations { get; set; } = new List<Import>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual UserRole Role { get; set; } = null!;
}
