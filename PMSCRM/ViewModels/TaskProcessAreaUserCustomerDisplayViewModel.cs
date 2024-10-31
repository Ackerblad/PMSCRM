namespace PMSCRM.ViewModels
{
    public class TaskProcessAreaUserCustomerDisplayViewModel
    {
        public Guid TaskProcessAreaUserCustomerId { get; set; }
        public string TaskName { get; set; }
        public string ProcessName { get; set; }
        public string AreaName { get; set; }
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
        public byte Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
