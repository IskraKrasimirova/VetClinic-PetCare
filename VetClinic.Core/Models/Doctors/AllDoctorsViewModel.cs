using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Core.Models.Doctors
{
    public class AllDoctorsViewModel
    {
        public const int DoctorsPerPage = 3;

        public string Department { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; }

        public int CurrentPage { get; init; } = 1;

        public int TotalDoctors { get; set; }

        public IEnumerable<string> Departments { get; set; }

        public IEnumerable<DoctorServiceModel> Doctors { get; set; }
    }
}
