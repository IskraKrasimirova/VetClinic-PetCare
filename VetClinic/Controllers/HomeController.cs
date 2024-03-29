﻿using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Extensions;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public IActionResult Index()
        {
            if (this.User.IsInRole(ClientRoleName))
            {
                var userId = this.User.GetId();
                this.ViewBag.ClientFullName = this.homeService.GetClientFullName(userId);
            }

            if (this.User.IsInRole(DoctorRoleName))
            {
                return RedirectToAction("Index", "Home", new { Area = "Doctor" });
            }

            if (this.User.IsInRole(AdminRoleName))
            {
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}