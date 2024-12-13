using System;
using System.Collections.Generic;

namespace MachineTestCamp6.Model;

public partial class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public int VendorId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<AssetMain> AssetMains { get; set; } = new List<AssetMain>();

    public virtual Vendor Vendor { get; set; } = null!;
}
