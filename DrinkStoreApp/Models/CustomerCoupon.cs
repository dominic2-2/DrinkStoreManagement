using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class CustomerCoupon
{
    public int CustomerCouponId { get; set; }

    public int CustomerId { get; set; }

    public int CouponId { get; set; }

    public int? OrderId { get; set; }

    public byte Status { get; set; }

    public virtual Coupon Coupon { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
