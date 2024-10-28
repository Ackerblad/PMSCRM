using PMSCRM.Models;

namespace PMSCRM.ViewModels
{
    public class SearchResultsViewModel
    {
        public List<Area>? Areas { get; set; }
        public List<Customer>? Customers { get; set; }
        public List<Process>? Processes { get; set; }
        public List<Models.Task>? Tasks { get; set; }
        public List<User>? Users { get; set; }

        public List<string>? FilterArray { get; set; }

        public string? CurrentSort { get; set; }
        public string? CurrentSortDirection { get; set; }
        public string Query { get; set; }
    }
}
