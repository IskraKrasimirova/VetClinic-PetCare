using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using VetClinic.Data.Models;

namespace VetClinic.Test.Mocks
{
    public class SignInManagerMock
    {
        public static SignInManager<User> Instance
        {
            get
            {
                var signInManagerMock = new Mock<SignInManager<User>>(
                    UserManagerMock.Instance,
                    Mock.Of<IHttpContextAccessor>(),
                    Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                    null, null, null, null);

                return signInManagerMock.Object;
            }
        }
    }
}
