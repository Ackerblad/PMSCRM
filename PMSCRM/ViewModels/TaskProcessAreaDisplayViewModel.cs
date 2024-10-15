namespace PMSCRM.ViewModels
{
    public class TaskProcessAreaDisplayViewModel
    {
        public Guid TaskProcessAreaId { get; set; }
        public string TaskName { get; set; }
        public string ProcessName { get; set; }
        public string AreaName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
