using System;
using System.Collections.Generic;

namespace ChawlaClinic.DAL.Entities;

public partial class Patient
{
    public int PatientId { get; set; }

    public string CaseNo { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int AgeYears { get; set; }

    public int AgeMonths { get; set; }

    public string Gender { get; set; } = null!;

    public string GuardianName { get; set; } = null!;

    public string? Disease { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string Status { get; set; } = null!;

    public DateTime FirstVisit { get; set; }

    public int DiscountId { get; set; }

    public string? Description { get; set; }

    public virtual DiscountOption Discount { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
