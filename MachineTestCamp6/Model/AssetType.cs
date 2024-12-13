using System;
using System.Collections.Generic;

namespace MachineTestCamp6.Model;

public partial class AssetType
{
    public int AssetTypeId { get; set; }

    public string AssetTypeName { get; set; } = null!;
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<AssetMain> AssetMains { get; set; } = new List<AssetMain>();
}
