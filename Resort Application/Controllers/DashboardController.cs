using Microsoft.AspNetCore.Mvc;
using Resort_Application.ViewModels;
using Stripe;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Application.Common.Utility;
using White.Lagoon.Application.Services.Interface;

namespace Resort_Application.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookigRadialChartData()
        {
            return Json(await _dashboardService.GetTotalBookigRadialChartData());
        }
        public async Task<IActionResult> GetRegisteredUserChartData()
        {

            return Json(await _dashboardService.GetRegisteredUserChartData());
        }
        public async Task<IActionResult> GetRevenuedUserChartData()
        {
           return Json(await _dashboardService.GetRevenuedUserChartData()); 
        }
        public async Task<IActionResult> GetBookingPieChartData()
        {
            return Json(await _dashboardService.GetBookingPieChartData());
        }
        public async Task<IActionResult> GetMemberAndBookingLineChartData()
        {
            return Json(await _dashboardService.GetMemberAndBookingLineChartData());
        }
      

    }
}
