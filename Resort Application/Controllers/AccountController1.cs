using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Resort_Application.ViewModels;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Domain.Entities;

namespace Resort_Application.Controllers
{
    public class AccountController1 : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public  AccountController1(IUnitOfWork unitOfWork, 
                UserManager<ApplicationUser> userManager, 
                SignInManager<ApplicationUser> signInManager, 
                RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl
            };
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

    }
}
