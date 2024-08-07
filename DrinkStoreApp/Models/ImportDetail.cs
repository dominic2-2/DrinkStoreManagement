using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class ImportDetail
{
    public int ImportDetailId { get; set; }

    public int ImportId { get; set; }

    public int IngredientId { get; set; }

    public decimal Quantity { get; set; }

    public int UnitId { get; set; }

    public virtual Import Import { get; set; } = null!;

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
