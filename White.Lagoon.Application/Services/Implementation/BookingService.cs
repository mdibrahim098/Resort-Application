using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Application.Services.Interface;
using White.Lagoon.Domain.Entities;

namespace White.Lagoon.Application.Services.Implementation
{
    public class BookingService : IBookingService
    {

        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateBooking(Booking booking)
        {
           _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
        }

        public IEnumerable<Booking> GetAllBookings(string userId = "", string? statusFilterList = "")
        {
            IEnumerable<string> statusList = statusFilterList.ToLower().Split(',');
            if (!string.IsNullOrEmpty(statusFilterList) && !string.IsNullOrEmpty(userId))
            {
                return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()) &&
                        u.UserId == userId, includeProperties: "User,Villa");
            }
            else
            {
                if (!string.IsNullOrEmpty(statusFilterList))
                {
                    return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower())
                          , includeProperties: "User,Villa");
                }
                else if (!string.IsNullOrEmpty(userId))
                {
                    return _unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "User,Villa");
                }
            }

            return _unitOfWork.Booking.GetAll(includeProperties: "User,Villa");

        }

        public Booking GetBookingById(int BookingId)
        {
            return _unitOfWork.Booking.Get(u => u.Id == BookingId, includeProperties: "User,Villa");
        }
    }
}
