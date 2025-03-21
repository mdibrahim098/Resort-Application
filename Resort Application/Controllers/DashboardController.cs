﻿using Microsoft.AspNetCore.Mvc;
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

            return Json(GetRadialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth));
        }
        public async Task<IActionResult> GetRegisteredUserChartData()
        {
            var totalUser = _unitOfWork.User.GetAll();

            var countByCurrentMonth = totalUser.Count(u => u.CreateAt >= currentMonthStartDate
            && u.CreateAt <= DateTime.Now);

            var countByPreviousMonth = totalUser.Count(u => u.CreateAt >= previousMonthStartDate
             && u.CreateAt <= currentMonthStartDate);

            return Json(GetRadialChartDataModel(totalUser.Count(), countByCurrentMonth, countByPreviousMonth));
        }

        public async Task<IActionResult> GetRevenuedUserChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.Status != SD.StatusPending
           || u.Status == SD.StatusCancelled);

            var totalRevenue =Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));

            var countByCurrentMonth = totalBookings.Where(u => u.BookingDate >= currentMonthStartDate
           && u.BookingDate <= DateTime.Now).Sum(u=>u.TotalCost);

            var countByPreviousMonth = totalBookings.Where(u => u.BookingDate >= previousMonthStartDate
             && u.BookingDate <= currentMonthStartDate).Sum(u => u.TotalCost);

            return Json(GetRadialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth));
        }


        public async Task<IActionResult> GetBookingPieChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&
            (u.Status == SD.StatusCancelled || u.Status == SD.StatusCancelled ));
           
            var customerWithOneBooking = totalBookings.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

            int bookingsByNewCustomer = customerWithOneBooking.Count();
            int bookingByReturingCustomer = totalBookings.Count() - bookingsByNewCustomer;

            PieChartVM pieChartVM = new()
            {
                Labels = new string[] {"New Customer Bookings","returning Customer Bookings"},
                Series = new decimal[] {bookingsByNewCustomer, bookingByReturingCustomer}
            };

            return Json(pieChartVM);
        }
        private static RadialBarChartVM GetRadialChartDataModel(int totaCount, double currentMonthCount, double prevMonthCount)
        {
            RadialBarChartVM radialBarChartVM = new();

            int increaseDrercreaseRatio = 100;
            if (prevMonthCount != 0)
            {
                increaseDrercreaseRatio = Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);
            }

            radialBarChartVM.TotalCount = totaCount;
            radialBarChartVM.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
            radialBarChartVM.HasRatioIncreased = currentMonthCount > prevMonthCount;
            radialBarChartVM.Series = new int[] { increaseDrercreaseRatio };
            return radialBarChartVM;
        }


    }
}
