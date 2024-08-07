using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string IngredientName { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal MinQuantity { get; set; }

    public int UnitId { get; set; }

    public virtual ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();

    public virtual ICollection<RecipeDetail> RecipeDetails { get; set; } = new List<RecipeDetail>();

    public virtual Unit Unit { get; set; } = null!;
}
