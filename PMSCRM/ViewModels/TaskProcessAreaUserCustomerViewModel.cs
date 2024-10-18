using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMSCRM.ViewModels
{
    public class TaskProcessAreaUserCustomerViewModel
    {
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.AddSeconds(-DateTime.Now.Second).AddMilliseconds(-DateTime.Now.Millisecond);
        public DateTime EndDate { get; set; } = DateTime.Now.AddSeconds(-DateTime.Now.Second).AddMilliseconds(-DateTime.Now.Millisecond);
        public TaskStatus Status { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public IEnumerable<TaskProcessAreaDisplayViewModel> ExistingConnections { get; set; }
        public IEnumerable<TaskProcessAreaDisplayViewModel> TaskProcessAreas { get; set; }
    }
}

