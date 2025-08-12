using System;
using System.Collections.Generic;

namespace DAL;

public partial class PantherType
{
    public int PantherTypeId { get; set; }

    public string? PantherTypeName { get; set; }

    public string? Origin { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<PantherProfile> PantherProfiles { get; set; } = new List<PantherProfile>();
}
