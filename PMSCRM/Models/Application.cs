using System;
using System.Collections.Generic;

namespace PMSCRM.Models;

public partial class Application
{
    public Guid ApplicationId { get; set; }

    public string Data { get; set; } = null!;
}
