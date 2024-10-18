using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMSCRM.ViewModels
{
    public class TaskProcessAreaViewModel
    {
        public Guid TaskProcessAreaId { get; set; }
        public Guid TaskId { get; set; }
        public Guid ProcessId { get; set; }
        public Guid AreaId { get; set; }

        public IEnumerable<SelectListItem>? Tasks { get; set; }
        public IEnumerable<SelectListItem>? Processes { get; set; }
        public IEnumerable<SelectListItem>? Areas { get; set; }
    }
}
