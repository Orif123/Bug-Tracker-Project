﻿using BugTrackerProj.Data;
using BugTrackerProj.Service;
using BugTrackerProj.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerProj.Controllers
{
    [Authorize(Roles = "CompanyManager, Admin")]
    public class AdminRoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBugService _bugService;
        public AdminRoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IBugService bugService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _bugService = bugService;
        }
        [Authorize(Roles = "CompanyManager")]
        public IActionResult Index(MainPageViewModel model)
        {
            model.Bugs = _bugService.GetAllBugs().ToList();
            return View(model);
        }
        [Authorize(Roles = "CompanyManager")]
        public IActionResult Create()
        {
            var users = _bugService.GetUserNames().ToList();
            ViewBag.Users = users;
            var roles = _bugService.GetUserRoles().ToList();
            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserToRoleRegistraionViewModel model)
        {
            var IsRoleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (IsRoleExists)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                await _userManager.AddToRoleAsync(user, model.RoleName);
                return RedirectToAction("Index", "AdminRole");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Solved(string id)
        {
            if (User.IsInRole("Admin") || User.IsInRole("CompanyManager"))
            {
                _bugService.BugSolved(id);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }


    }
}
