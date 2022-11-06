// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using VetClinic.Data;
using VetClinic.Data.Models;
using static VetClinic.Data.ModelConstants.User;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly VetClinicDbContext _data;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            VetClinicDbContext data)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _data = data;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Full Name")]
            [StringLength(FullNameMaxLength,
                MinimumLength = FullNameMinLength,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            public string FullName { get; set; }

            [Required]
            [RegularExpression
                (PhoneNumberRegex, ErrorMessage = "The provided Phone Number is not valid or contains unnecessary white spaces!")]
            [Display(Name = "Phone Number")]
            [StringLength(PhoneNumberMaxLength,
                MinimumLength = PhoneNumberMinLength,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            public string PhoneNumber { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            string phoneNumberFromDB = this._data
                .Users
                .Select(u => u.PhoneNumber)
                .FirstOrDefault(pn => pn.Trim() == this.Input.PhoneNumber.Trim());

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                if (phoneNumberFromDB == null)
                {
                    User user = new()
                    {
                        Email = this.Input.Email,
                        UserName = this.Input.Email,
                        PhoneNumber = this.Input.PhoneNumber,
                        FullName = this.Input.FullName
                    };

                    IdentityResult result = await this._userManager.CreateAsync(user, this.Input.Password);

                    if (result.Succeeded)
                    {
                        await this._data.Clients
                            .AddAsync(new Client()
                            {
                                UserId = user.Id,
                                FullName = this.Input.FullName,
                            });
                        await this._userManager.AddToRoleAsync(user, ClientRoleName);
                        await this._signInManager.SignInAsync(user, isPersistent: false);
                        await this._data.SaveChangesAsync();

                        return this.LocalRedirect(returnUrl);
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Please enter a different phone number!");
                    return this.Page();
                }
            }

            //if (ModelState.IsValid)
            //{
            //    var user = CreateUser();

            //    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            //    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            //    var result = await _userManager.CreateAsync(user, Input.Password);

            //    if (result.Succeeded)
            //    {
            //        _logger.LogInformation("User created a new account with password.");

            //        var userId = await _userManager.GetUserIdAsync(user);
            //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //        var callbackUrl = Url.Page(
            //            "/Account/ConfirmEmail",
            //            pageHandler: null,
            //            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
            //            protocol: Request.Scheme);

            //        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
            //            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            //        if (_userManager.Options.SignIn.RequireConfirmedAccount)
            //        {
            //            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            //        }
            //        else
            //        {
            //            await _signInManager.SignInAsync(user, isPersistent: false);
            //            return LocalRedirect(returnUrl);
            //        }
            //    }
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError(string.Empty, error.Description);
            //    }
            //}

            // If we got this far, something failed, redisplay form
            return Page();
        }

        //private IdentityUser CreateUser()
        //{
        //    try
        //    {
        //        return Activator.CreateInstance<IdentityUser>();
        //    }
        //    catch
        //    {
        //        throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
        //            $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
        //            $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        //    }
        //}

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
