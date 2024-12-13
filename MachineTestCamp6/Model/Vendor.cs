using System;
using System.Collections.Generic;

namespace MachineTestCamp6.Model;

public partial class Vendor
{
    public int VendorId { get; set; }

    public string VendorName { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Address { get; set; } = null!;
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<AssetMain> AssetMains { get; set; } = new List<AssetMain>();
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
