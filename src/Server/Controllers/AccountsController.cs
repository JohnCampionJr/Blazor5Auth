﻿using System.Linq;
using System.Threading.Tasks;
using Blazor5Auth.Server.Models;
using Blazor5Auth.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blazor5Auth.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private static UserModel LoggedOutUser = new UserModel { IsAuthenticated = false };

        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            var newUser = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterResult { Successful = false, Errors = errors });
            }

            // Add all new users to the User role
            await _userManager.AddToRoleAsync(newUser, "User");

            // Add new users whose email starts with 'admin' to the Admin role
            if (newUser.Email.StartsWith("admin"))
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");
            }

            return Ok(new RegisterResult { Successful = true });
        }
    }
}
