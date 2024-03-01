using System;
using System.Collections.Generic;

namespace ChawlaClinic.DAL.Entities;

public partial class Sequence
{
    public string Name { get; set; } = null!;

    public uint NextValue { get; set; }
}
