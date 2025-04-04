using Company.G01.DAL.Models;
using Company.G01.PL.Dtos;
using Company.G01.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.G01.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager) 
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
              users =  _userManager.Users.Select(U => new UserToReturnDto
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    Email = U.Email,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Roles =  _userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
               users = _userManager.Users.Select(U => new UserToReturnDto
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    Email = U.Email,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }


            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var user = await _userManager.FindByIdAsync(id);

            if (user is null) return NotFound(new { StatusCode = 404, Message = $"User with Id : {id} is not found " });

            var dto = new UserToReturnDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = _userManager.GetRolesAsync(user).Result
            };

            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturnDto model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invaild Operations !");
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return BadRequest("Invaild Operations !");
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) 
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(viewName, model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invaild Operations !");
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return BadRequest("Invaild Operations !");

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

    }
}
