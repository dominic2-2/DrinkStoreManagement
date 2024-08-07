using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateOnly? BirthDate { get; set; }

    public byte IsRegistered { get; set; }

    public virtual ICollection<CustomerCoupon> CustomerCoupons { get; set; } = new List<CustomerCoupon>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
