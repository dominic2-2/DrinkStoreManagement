using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class RecipeDetail
{
    public int RecipeDetailId { get; set; }

    public int RecipeId { get; set; }

    public int IngredientId { get; set; }

    public decimal Quantity { get; set; }

    public int UnitId { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
