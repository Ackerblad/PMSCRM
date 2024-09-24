using System;
using System.Collections.Generic;

namespace PMSCRM.Models;
public partial class Role
{
    public Guid RoleId { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public Role()
    {
    }
}
