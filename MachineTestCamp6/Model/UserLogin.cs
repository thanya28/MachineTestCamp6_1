using System;
using System.Collections.Generic;

namespace MachineTestCamp6.Model;

public partial class UserLogin
{
    public int LoginId { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool? IsActive { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual UserRegistration User { get; set; } = null!;
}
