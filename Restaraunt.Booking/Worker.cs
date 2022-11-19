using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly Restaurant _restaurant;
        private readonly ILogger _logger;//добавляем логгер

        public Worker(IBus bus, Restaurant restaurant,
        ILogger<BackgroundService> logger)
        {
            _bus = bus;
            _restaurant = restaurant;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);
                Console.WriteLine("Привет! Желаете забронировать столик?");
                var result = await _restaurant.BookFreeTableAsync(1);
                //забронируем с ответом по смс
                var id = NewId.NextGuid();
                await _bus.Publish(new TableBooked(id, result ?? false),
                    context => context.Durable = false, stoppingToken);
                _logger.Log(LogLevel.Information, $"[OrderId: {id}]");
                _logger.Log(LogLevel.Debug, $"[OrderId: {id}]");//Уровень Debug
                Console.WriteLine(id);
            }
        }
    }
    //public class Worker : BackgroundService
    //{
    //    private readonly IBus _bus;
    //
    //    public Worker(IBus bus)
    //    {
    //        _bus = bus;
    //    }
    //
    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        Console.OutputEncoding = System.Text.Encoding.UTF8;
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            await Task.Delay(1000, stoppingToken);
    //            Console.WriteLine("Привет! Желаете забронировать столик?");
    //            var b = Guid.NewGuid();
    //
    //            var dateTime = DateTime.Now;
    //            await _bus.Publish(
    //                //(IBookingRequest)new BookingRequest(NewId.NextGuid(), NewId.NextGuid(), null, dateTime),
    //                (IBookingRequest)new BookingRequest(b, NewId.NextGuid(), null, dateTime),
    //                stoppingToken);
    //        }
    //    }
    //}
}

