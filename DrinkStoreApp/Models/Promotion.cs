using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string PromotionName { get; set; } = null!;

    public string PromotionType { get; set; } = null!;

    public decimal? DiscountValue { get; set; }

    public int? BuyQuantity { get; set; }

    public int? FreeQuantity { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? Description { get; set; }

    public int Priority { get; set; }

    public byte Status { get; set; }

    public virtual ICollection<ProductPromotion> ProductPromotions { get; set; } = new List<ProductPromotion>();
}
