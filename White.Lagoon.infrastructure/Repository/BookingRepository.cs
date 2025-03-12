using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Application.Common.Utility;
using White.Lagoon.Domain.Entities;
using White.Lagoon.infrastructure.Data;

namespace White.Lagoon.infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {

        private readonly ApplicationDbContext _db;

        public BookingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Booking entity)
        {
            _db.Bookings.Update(entity);
        }

        public void UpdateStatus(int bookingId, string bookingStatus, int villaNumber=0)
        {
           var bookingFromnDb = _db.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (bookingFromnDb != null)
            {
                bookingFromnDb.Status = bookingStatus;
                if (bookingStatus == SD.StatusCheckedIn)
                {
                    bookingFromnDb.VillaNumber = villaNumber;
                    bookingFromnDb.ActualCheckInDate = DateTime.Now;
                }
                if(bookingStatus == SD.StatusCompleted)
                {
                    bookingFromnDb.ActualCheckOutDate = DateTime.Now;
                } 
            }
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromnDb = _db.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (bookingFromnDb != null)
            {
                if(!string.IsNullOrEmpty(sessionId))
                {
                    bookingFromnDb.StripeSessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    bookingFromnDb.StripePaymentIntentId = paymentIntentId;
                    bookingFromnDb.PaymentDate = DateTime.Now;
                    bookingFromnDb.IsPaymentSuccessful = true;
                }
            }
         }
    }
}
