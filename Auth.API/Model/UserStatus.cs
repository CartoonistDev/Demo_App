using System.ComponentModel;

namespace Auth.API.Model;

public enum UserStatus
{
    [Description("Locked")]
    Locked = 1,
    [Description("Approved")]
    Approved = 2,
    [Description("Disabled")]
    Disabled = 3,
    [Description("Pending")]
    Pending = 3,
}

public enum Roles
{
    [Description("Customer")]
    Customer = 0,
    [Description("Administrator")]
    Administrator = 1
}
