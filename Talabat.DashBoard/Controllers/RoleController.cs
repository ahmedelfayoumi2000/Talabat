using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.DashBoard.Models;

namespace Talabat.DashBoard.Controllers
{
    public class RoleController(RoleManager<IdentityRole> _roleManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            // Get All The Roles
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                    TempData["SuccessMessage"] = "Role created successfully!"; 
                }
                else
                {
                    TempData["ErrorMessage"] = "Role already exists!"; 
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                TempData["SuccessMessage"] = "Role deleted successfully!"; 
            }
            else
            {
                TempData["ErrorMessage"] = "Role not found!"; 
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var mappedRoled = new RoleViewModel
            {
                Name = role.Name
            };
            return View(mappedRoled);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExists)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);
                    role.Name = model.Name;
                    await _roleManager.UpdateAsync(role);
                    TempData["SuccessMessage"] = "Role Edited successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Role already exists!";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}