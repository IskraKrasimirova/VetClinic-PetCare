using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Data.Models;

namespace VetClinic.Test.Mocks
{
    public class UserManagerMock
    {
        public static UserManager<User> Instance
        {
            get
            {
                var store = new Mock<IUserStore<User>>();
                var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
                userManager.Object.UserValidators.Add(new UserValidator<User>());
                userManager.Object.PasswordValidators.Add(new PasswordValidator<User>());

                userManager.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
                userManager.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

                return userManager.Object;
            }
        }
    }
}
