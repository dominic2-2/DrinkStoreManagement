using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class Unit
{
    public int UnitId { get; set; }

    public string UnitName { get; set; } = null!;

    public virtual ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<RecipeDetail> RecipeDetails { get; set; } = new List<RecipeDetail>();
}
