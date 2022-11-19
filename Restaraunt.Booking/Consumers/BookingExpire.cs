using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaraunt.Booking.Consumers
{
    public class BookingExpire : IBookingExpire
    {
        private readonly RestaurantBooking _instance;

        public BookingExpire(RestaurantBooking instance)
        {
            _instance = instance;
        }

        public new Guid OrderId => _instance.OrderId;
    }

}

