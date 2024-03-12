using System;
using System.Collections.Generic;

namespace ChawlaClinic.DAL.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string Code { get; set; } = null!;

    public int AmountPaid { get; set; }

    public DateTime DateTime { get; set; }

    public int PatientId { get; set; }

    public int DiscountId { get; set; }

    public string Status { get; set; } = null!;

    public virtual DiscountOption Discount { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
