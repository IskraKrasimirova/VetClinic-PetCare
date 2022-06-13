using System.Collections.Generic;
using System.Security.Claims;

namespace VetClinic.Test.Mocks
{
    public class ClaimsPrincipalMock
    {
        public static ClaimsPrincipal Instance(string userId = "TestUserId")
        {
            var fakeClaims = new List<Claim>()
            {
               new Claim(ClaimTypes.NameIdentifier, userId),
               new Claim(ClaimTypes.Authentication, ClaimsIdentity.DefaultNameClaimType, userId),
               new Claim(ClaimsIdentity.DefaultRoleClaimType, userId)
            };

            var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");

            var mockClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);

            return mockClaimsPrincipal;
        }
    }
}
