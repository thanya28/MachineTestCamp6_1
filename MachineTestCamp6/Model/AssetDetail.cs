using System;
using System.Collections.Generic;

namespace MachineTestCamp6.Model;

public partial class AssetDetail
{
    public int AssetDetailId { get; set; }

    public string SerialNumber { get; set; } = null!;

    public string Specification { get; set; } = null!;

    public string WarrantyPeriod { get; set; } = null!;

    public string? CurrentOwner { get; set; }

    public DateTime LastUpdated { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<AssetMain> AssetMains { get; set; } = new List<AssetMain>();
}
