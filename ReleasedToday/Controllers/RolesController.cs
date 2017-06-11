using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ReleasedToday.Models;
using ReleasedToday.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReleasedToday.Controllers
{
    [Authorize(Roles="Administrator")]
    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string description)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole { Name = name, Description = description };
                await _roleManager.CreateAsync(role);

                return RedirectToAction("Index");
            }

            return View(name, description);
        }

        public IActionResult Join()
        {
            UserRoleViewModel model = new UserRoleViewModel
            {
                RoleList = new SelectList(_roleManager.Roles, "Id", "Name"),
                UserList = new SelectList(_userManager.Users, "Id", "UserName")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Join(string userId, string roleId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var role = await _roleManager.FindByIdAsync(roleId);
                await _userManager.AddToRoleAsync(user, role.Name);

                return RedirectToAction("Details", new { roleName = role.Name});
            }

            return View();
        }

        public async Task<IActionResult> Details(string roleName)
        {
            if(String.IsNullOrEmpty(roleName))
            {
                return NotFound();
            }

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            ViewBag.Role = roleName;

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string role, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRoleAsync(user, role);

            return RedirectToAction("Details", new { roleName = role });
        }
    }
}