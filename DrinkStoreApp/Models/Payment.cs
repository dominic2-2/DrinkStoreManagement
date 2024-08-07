using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentType { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal ChangeDue { get; set; }

    public string? Note { get; set; }

    public byte PaymentStatus { get; set; }

    public byte CouponApplied { get; set; }

    public string? TransactionId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
