using Microsoft.AspNetCore.Mvc;
using Resort_Application.ViewModels;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Application.Common.Utility;

namespace Resort_Application.Controllers
{
    public class DashboardController : Controller
    {
        private readonly  IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Year == 1? 12 : DateTime.Now.Month - 1;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookigRadialChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u=> u.Status != SD.StatusPending
            || u.Status == SD.StatusCancelled);

            var countByCurrentMonth = totalBookings.Count(u => u.BookingDate >= currentMonthStartDate 
            &&  u.BookingDate <= DateTime.Now);

            var countByPreviousMonth = totalBookings.Count(u => u.BookingDate >= previousMonthStartDate
             && u.BookingDate <= currentMonthStartDate);

            RadialBarChartVM radialBarChartVM = new();

            int increaseDrercreaseRatio = 100;
            if (countByPreviousMonth != 0)
            {
                increaseDrercreaseRatio = Convert.ToInt32((countByCurrentMonth - countByPreviousMonth) / countByPreviousMonth * 100);
            }

            radialBarChartVM.TotalCount = totalBookings.Count();
            radialBarChartVM.CountInCurrentMonth = countByCurrentMonth;
            radialBarChartVM.HasRatioIncreased = currentMonthStartDate > previousMonthStartDate;
            radialBarChartVM.Series = new int[] { increaseDrercreaseRatio };

            return Json(radialBarChartVM);
        }



    }
}
