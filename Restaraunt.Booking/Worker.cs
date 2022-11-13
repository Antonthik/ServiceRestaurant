using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaraunt.Booking
{
    /// <summary>
    /// Генерация заказов и отправка
    /// </summary>
    //public class Worker : BackgroundService
    //{
    //    private readonly IBus _bus;
    //    private readonly Restaurant _restaurant;
    //
    //    public Worker(IBus bus, Restaurant restaurant)
    //    {
    //        _bus = bus;
    //        _restaurant = restaurant;
    //    }
    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        Console.OutputEncoding = System.Text.Encoding.UTF8;
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            await Task.Delay(10000, stoppingToken);
    //            Console.WriteLine("Привет! Желаете забронировать столик?");
    //            var result = await _restaurant.BookFreeTableAsync(1);
    //            //забронируем с ответом по смс
    //            await _bus.Publish(new TableBooked(NewId.NextGuid(), NewId.NextGuid(), result ?? false),
    //                context => context.Durable = false, stoppingToken);
    //        }
    //    }
    //}
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;

        public Worker(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                Console.WriteLine("Привет! Желаете забронировать столик?");
                var b = Guid.NewGuid();

                var dateTime = DateTime.Now;
                await _bus.Publish(
                    //(IBookingRequest)new BookingRequest(NewId.NextGuid(), NewId.NextGuid(), null, dateTime),
                    (IBookingRequest)new BookingRequest(b, NewId.NextGuid(), null, dateTime),
                    stoppingToken);
            }
        }
    }
}

