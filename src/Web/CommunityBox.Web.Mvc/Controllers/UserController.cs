using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.Api.WebContracts.Identities;
using CommunityBox.Web.Mvc.Clients.Gateway;
using CommunityBox.Web.Mvc.ViewModels.Identities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBox.Web.Mvc.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public const string Route = "User";
        private readonly IGatewayClient _gatewayClient;
        private readonly IMapper _mapper;

        public UserController(IGatewayClient gatewayClient, IMapper mapper)
        {
            _gatewayClient = gatewayClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string userId)
        {
            if (userId == null)
                return RedirectToAction("Error", "Home");
            
            var user = await _gatewayClient.GetUserAsync(userId);

            if (user == null)
                return RedirectToAction("Error", "Home");
            
            var retval = _mapper.Map<UserViewModel>(user);

            if (userId == User.Claims.First(f => f.Type == "uid").Value)
                retval.IsMyProfile = true;
            
            return View(retval);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel viewModel)
        {
            var request = _mapper.Map<LogInContract>(viewModel);
            var response = await _gatewayClient.AuthAsync(request);
            var auth = _mapper.Map<AuthViewModel>(response);

            await AuthenticateAsync(auth);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task AuthenticateAsync(AuthViewModel auth)
        {
            var claims = new List<Claim>
            {
                new("uid", auth.UserId),
                new("token", auth.Token),
                new("email", auth.Email)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}