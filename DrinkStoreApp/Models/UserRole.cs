using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class UserRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
