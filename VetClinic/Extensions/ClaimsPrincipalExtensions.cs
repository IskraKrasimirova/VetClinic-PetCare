using System.Security.Claims;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public static bool IsClient(this ClaimsPrincipal user)
            => user.IsInRole(ClientRoleName);

        public static bool IsDoctor(this ClaimsPrincipal user)
            => user.IsInRole(DoctorRoleName);

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdminRoleName);
        }
    }
}
