using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Models;

public partial class Role
{
    public Guid RoleId { get; set; }
    [Required]

    public Guid CompanyId { get; set; }
    [Required]
    [StringLength(50)]

    public string Name { get; set; } = null!;
    [Required]

    public DateTime Timestamp { get; set; }

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
