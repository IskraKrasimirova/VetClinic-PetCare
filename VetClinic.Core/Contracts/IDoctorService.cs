using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Models.Doctors;

namespace VetClinic.Core.Contracts
{
    public interface IDoctorService
    {
        AllDoctorsViewModel All(string departmentName, string searchTerm, int currentPage = 1,
            int doctorsPerPage = int.MaxValue);

        AvailableDoctorsServiceModel ByDepartment(AvailableDoctorsServiceModel query);

        DoctorDetailsServiceModel Details(string id);

        bool DoctorExists(string fullName, string phoneNumber);

        string Register(DoctorFormModel doctorModel);

        string Create(
            string fullName,
            string profileImage, 
            string phoneNumber, 
            string email, 
            string description, 
            int departmentId,
            string userId);
    }
}
