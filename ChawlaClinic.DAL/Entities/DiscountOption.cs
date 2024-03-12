using System;
using System.Collections.Generic;

namespace ChawlaClinic.DAL.Entities;

public partial class DiscountOption
{
    public int DiscountId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
