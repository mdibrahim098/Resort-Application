﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Application.Common.Utility;
using White.Lagoon.Domain.Entities;
using Session = Stripe.Checkout.Session;
using SessionService = Stripe.Checkout.SessionService;


namespace Resort_Application.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        public IActionResult FinalizeBooking(int villaId,DateOnly checkInDate,int nights)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _unitOfWork.User.Get(u => u.Id == userId);


            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.Villa.Get(u => u.Id == villaId, includeProperties: "VillaAmenity"),
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
                UserId = userId,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name
            };
            booking.TotalCost = booking.Villa.Price * nights;
            return View(booking);
        }


        [Authorize]
        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking)
        {

            var villa = _unitOfWork.Villa.Get(u => u.Id == booking.VillaId);
            booking.TotalCost = villa.Price * booking.Nights;

            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;

            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();

            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {

                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Booking/BookingConfirmation?bookingId={booking.Id}",
                CancelUrl = domain + $"Booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}",
            };
            
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)booking.TotalCost * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = villa.Name,
                        //Images = new List<string> { villa.ImageUrl },
                    },
                },
                Quantity = 1,
            });
           

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.Booking.UpdateStripePaymentID(booking.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

           
        }

        [Authorize]
        public IActionResult BookingConfirmation(int bookingid)
        {
            return View(bookingid);
        }


    }
}
