using System.Collections.Generic;
using System.Security.Claims;

namespace VetClinic.Test.Mocks
{
    public class ClaimsPrincipalMock
    {
        public static ClaimsPrincipal Instance(string userId = "TestUserId", string Role = null)
        {
            var fakeClaims = new List<Claim>()
            {
               new Claim(ClaimTypes.NameIdentifier, userId),
               //new Claim(ClaimTypes.Authentication, ClaimsIdentity.DefaultNameClaimType, userId),
               //new Claim(ClaimTypes.Authentication, ClaimsIdentity.DefaultNameClaimType, userId, Role),
               new Claim(ClaimsIdentity.DefaultRoleClaimType, userId),
            };

            var fakeIdentity = new ClaimsIdentity(fakeClaims, authenticationType: Role, nameType: Role, roleType: Role);

            var mockClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);

            return mockClaimsPrincipal;
        }
    }
}
