using System.Collections.Generic;
using PMSCRM.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMSCRM.ViewModels
{
    public class ProcessViewModel
    {
        public Process Process { get; set; } = new Process();
        public List<SelectListItem> Areas { get; set; } = new List<SelectListItem>();
    }
}
