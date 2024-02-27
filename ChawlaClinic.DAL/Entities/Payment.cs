using System;
using System.Collections.Generic;

namespace ChawlaClinic.DAL.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int Amount { get; set; }

    public DateTime DateTime { get; set; }

    public int PatientId { get; set; }

    public int? DiscountId { get; set; }

    public virtual DiscountOption? Discount { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
