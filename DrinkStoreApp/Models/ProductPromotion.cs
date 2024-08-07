using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class ProductPromotion
{
    public int ProductPromotionId { get; set; }

    public int ProductId { get; set; }

    public int PromotionId { get; set; }

    public int? OrderDetailId { get; set; }

    public virtual OrderDetail? OrderDetail { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;
}
