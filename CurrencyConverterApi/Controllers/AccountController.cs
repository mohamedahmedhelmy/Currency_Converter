using Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Text.RegularExpressions;

namespace CurrencyConverterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context; 
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (model == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                if (EmailExistes(model.Email))
                {
                    return BadRequest("Email is not Available");
                }
                if (!IsEmailValid(model.Email))
                {
                    return BadRequest("Email Is Not Vaild");
                }


                var user = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok("Register Success");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        /*
         Email ===> Admin@Admin.com
         password ====>Admin123@
         */
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (model == null)
                return NotFound();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound($"No user was found with Email: {model.Email}");

            var result = await _signInManager.
                PasswordSignInAsync(user, model.Password, model.RememberMe, true);
            if (result.Succeeded)
                return Ok("Login Success");
            else if (result.IsLockedOut)
                return Unauthorized("User Account Is Locked");
            else
                return StatusCode(StatusCodes.Status204NoContent);

        }
        private bool EmailExistes(string Email)
        {
            return _context.Users.Any(u => u.Email == Email);
        }
        private bool IsEmailValid(string Email)
        {
            Regex regex = new Regex(@"(\w+\@+\w+.com)|(\w+\@+\w+.net)");
            if (regex.IsMatch(Email))
            {
                return true;
            }
            return false;
        }

    }
}
