using Authentication.Apis.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication.Apis.Controllers
{
    //[Route("api/[controller]")]
    [Route("accounts/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationInputModel registrationInput)
        {
            var user = await _userManager.FindByNameAsync(registrationInput.UserName);
            if (user != null)
            {
                return BadRequest("User already exists.");
            }

            user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registrationInput.UserName,
                Email = registrationInput.Email
            };

            var result = await _userManager.CreateAsync(user, registrationInput.Password);
            if (!result.Succeeded)
            {
                var resultErrors = result.Errors.Select(e => e.Description);
                return BadRequest(string.Join("\n", resultErrors));
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginInputModel loginInput)
        {
            var user = await _userManager.FindByNameAsync(loginInput.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginInput.Password))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return Ok("Login successful");
            }

            return BadRequest("Invalid Username or Password");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logout Successful.");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details()
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _userManager.FindByIdAsync(loggedInUserId));
        }
    }
}
