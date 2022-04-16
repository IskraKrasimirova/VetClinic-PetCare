using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Areas.Doctor.Controllers
{
    [Area(DoctorRoleName)]
    [Authorize(Roles = DoctorRoleName)]
    public abstract class DoctorsController : Controller
    {
    }
}
