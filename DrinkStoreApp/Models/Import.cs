using System;
using System.Collections.Generic;

namespace DrinkStoreApp.Models;

public partial class Import
{
    public int ImportId { get; set; }

    public DateTime ImportDate { get; set; }

    public decimal AmountTaken { get; set; }

    public decimal? TotalCost { get; set; }

    public decimal? AmountRemaining { get; set; }

    public string? Description { get; set; }

    public int CreatedBy { get; set; }

    public int? ApprovedBy { get; set; }

    public int Status { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();
}
