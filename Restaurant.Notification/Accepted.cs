using System;

namespace Restaurant.Notification
{
    /// <summary>
    /// Акцепт для подверждения заказа.
    /// Если прошло бронирование и кухня,тогда заказ подтверждается
    /// </summary>
    [Flags]
    
    public enum Accepted
    {
        Rejected = 0,
        Kitchen = 1,
        Booking = 2,
        All = Kitchen | Booking
    }
}