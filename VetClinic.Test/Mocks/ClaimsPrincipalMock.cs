using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace VetClinic.Test.Mocks
{
    public class ClaimsPrincipalMock
    {
        public static ClaimsPrincipal Instance(string userId = "TestUserId", string Role = null)
        {
            var fakeClaims = new List<Claim>()
            {
               new Claim(ClaimTypes.NameIdentifier, userId),
               new Claim(ClaimTypes.Role, Role, null),
            };

            var fakeIdentity = new ClaimsIdentity(fakeClaims, authenticationType: Role);

            var mockClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(fakeIdentity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(mockClaimsPrincipal);
            return mockClaimsPrincipal;
        }
    }
}
