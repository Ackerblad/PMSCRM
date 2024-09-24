﻿using System;
using System.Collections.Generic;

namespace PMSCRM.Models;

public partial class TaskProcessAreaUserCustomer
{
    public Guid TaskProcessAreaUserCustomerId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid TaskProcessAreaId { get; set; }

    public Guid UserId { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public byte Status { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual TaskProcessArea TaskProcessArea { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public TaskProcessAreaUserCustomer()
    {
    }

    public TaskProcessAreaUserCustomer(Guid taskProcessAreaUserCustomerId, Guid companyId, Guid taskProcessAreaId, Guid userId, Guid customerId, DateTime startDate, DateTime endDate, byte status, DateTime? timestamp)
    {
        TaskProcessAreaUserCustomerId = taskProcessAreaUserCustomerId;
        CompanyId = companyId;
        TaskProcessAreaId = taskProcessAreaId;
        UserId = userId;
        CustomerId = customerId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        Timestamp = timestamp;
    }
}
