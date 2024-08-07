using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int ProductId { get; set; }

    public string RecipeName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int RoleId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<RecipeDetail> RecipeDetails { get; set; } = new List<RecipeDetail>();

    public virtual UserRole Role { get; set; } = null!;
}
