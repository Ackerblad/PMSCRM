using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Models;

public partial class Area
{
    public Guid AreaId { get; set; }
    [Required]
    public Guid CompanyId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }
    [Required]
    public DateTime Timestamp { get; set; }

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<TaskProcessArea> TaskProcessAreas { get; set; } = [];
}
