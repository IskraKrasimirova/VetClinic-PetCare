using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public abstract class AdminController : Controller
    {
    }
}
