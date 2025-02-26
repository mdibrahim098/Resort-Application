using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Resort_Application.Models;
using Resort_Application.ViewModels;
using White.Lagoon.Application.Common.Interfaces;

namespace Resort_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),

            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

      
        public IActionResult Error()
        {
            return View();
        }



    }
}
