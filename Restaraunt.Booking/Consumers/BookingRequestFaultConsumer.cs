using MassTransit;
using Restaurant.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaraunt.Booking.Consumers
{
    //Данный блок реагирует на события при ошибки бронирования
    //Консумер принимает Fault(ошибку) от BookingRequest, слушает отдельную очередь с ошибками
    public class BookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            Console.WriteLine($"[OrderId {context.Message.Message.OrderId}] Отмена в зале");
            return Task.CompletedTask;
        }
    }
}
