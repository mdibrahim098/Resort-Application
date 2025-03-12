using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using White.Lagoon.Domain.Entities;

namespace White.Lagoon.Application.Common.Utility
{
   public static class SD
    {

        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static int VillaRoomsAvailable_Count(int villaId,
            List<VillaNumber> villaNumberList,DateOnly checkInDate, int nights,
            List<Booking> bookings)
        {
            List<int> bookingInDate = new();

            var roomsInVilla = villaNumberList.Where(v => v.VillaId == villaId).Count();

            for(int i=0; i<nights; i++)
            {
                var villasBook = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i) &&
                u.CheckOutDate > checkInDate.AddDays(i) && u.VillaId == villaId);
                foreach (var booking in villasBook)
                {
                    if(!bookingInDate.Contains(booking.Id))
                    {
                        bookingInDate.Add(booking.Id);
                    }
                }

                var totalAvailableRooms = roomsInVilla - bookingInDate.Count;
                if(totalAvailableRooms == 0)
                {
                    return 0;
                }
            }

        }


    }
}
