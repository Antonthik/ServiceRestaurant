
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaraunt.Booking.Consumers;
using Restaurant.Booking;
using Restaurant.Messages.InMemoryDb;
using Restaurant.Notification;
using System.Diagnostics;


namespace Restaraunt.Booking
{
    /// <summary>
    /// Массовая отправка сообщений на бронирование
    /// </summary>
    //internal class Program
    //{
    //    private static async Task Main(string[] args)
    //    {
    //        Console.OutputEncoding = System.Text.Encoding.UTF8;
    //        var rest = new Restaurant();
    //        while (true)
    //        {
    //            await Task.Delay(10000);
    //
    //            //считаем что если уж позвонили, то столик забронировать хотим
    //            Console.WriteLine("Привет! Желаете забронировать столик?");
    //
    //            var stopWatch = new Stopwatch();
    //            stopWatch.Start(); //замерим потраченное нами время на бронирование,
    //                               //ведь наше время - самое дорогое что у нас есть
    //
    //            rest.BookFreeTableAsync(1); //забронируем с ответом по смс - и отправим брокеру сообщение
    //
    //            Console.WriteLine("Спасибо за Ваше обращение!"); //клиента всегда нужно порадовать благодарностью
    //            stopWatch.Stop();
    //            var ts = stopWatch.Elapsed;
    //            Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}"); //выведем потраченное нами время
    //        }
    //    }
    //}


    //public static class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        Console.OutputEncoding = System.Text.Encoding.UTF8;
    //        CreateHostBuilder(args).Build().Run();
    //    }
    //
    //    private static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureServices((hostContext, services) =>
    //            {
    //                services.AddMassTransit(x =>
    //                {
    //                    x.UsingRabbitMq((context, cfg) =>
    //                    {
    //                        cfg.ConfigureEndpoints(context);
    //                    });
    //                });
    //                services.AddMassTransitHostedService(true);
    //
    //                services.AddTransient<Restaurant>();
    //
    //                services.AddHostedService<Worker>();
    //            });
    //}
    //public static class Program
    //{
    //
    //    public static void Main(string[] args)
    //    {
    //        Console.OutputEncoding = System.Text.Encoding.UTF8;
    //        CreateHostBuilder(args).Build().Run();
    //    }
    //
    //    private static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureServices((hostContext, services) =>
    //            {
    //                services.AddMassTransit(x =>
    //                {
    //
    //                    x.AddConsumer<RestaurantBookingRequestConsumer>()
    //                        .Endpoint(e =>
    //                        {
    //                            e.Temporary = true;
    //                        });
    //
    //                    x.AddConsumer<BookingRequestFaultConsumer>(config => 
    //                        {
    //                            config.UseScheduledRedelivery(r =>
    //                            {
    //                                r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20), 
    //                                    TimeSpan.FromSeconds(30));//В случае долгой остановки службы включаем планировщик для повторной отправки по расписанию и не держать в памяти
    //                            });
    //
    //                            config.UseMessageRetry(r =>
    //                            {
    //                                r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));//Повтор сообщения в случае ошибки
    //                               // r.Handle<ArgumentNullException>();//Можно конфигурировать для конкретного вида исключения
    //                            });
    //                        })
    //                        .Endpoint(e =>
    //                        {
    //                            e.Temporary = true;
    //                        });
    //
    //                    x.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()
    //                        .Endpoint(e => e.Temporary = true)
    //                        .InMemoryRepository();
    //
    //                    x.AddDelayedMessageScheduler();
    //
    //                    x.UsingRabbitMq((context, cfg) =>
    //                    {
    //                        cfg.UseDelayedMessageScheduler();
    //                        cfg.UseInMemoryOutbox();
    //                        cfg.ConfigureEndpoints(context);
    //                    });
    //
    //                });
    //
    //                services.AddMassTransitHostedService();
    //
    //                services.AddTransient<RestaurantBooking>();
    //                services.AddTransient<RestaurantBookingSaga>();
    //                services.AddTransient<Restaurant>();
    //
    //                services.AddHostedService<Worker>();
    //            });
    //}
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<RestaurantBookingRequestConsumer>();
                        //   x.AddConsumer<BookingRequestFaultConsumer>();

                        x.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()
                            .InMemoryRepository();

                        x.AddDelayedMessageScheduler();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Durable = false;
                            cfg.UseDelayedMessageScheduler();
                            cfg.UseInMemoryOutbox();
                            cfg.ConfigureEndpoints(context);
                        });

                    });

                    services.AddTransient<RestaurantBooking>();
                    services.AddTransient<RestaurantBookingSaga>();
                    services.AddTransient<Restaurant>();
                    services.AddSingleton<IInMemoryRepository<BookingRequestModel>, InMemoryRepository<BookingRequestModel>>();

                    services.AddHostedService<Worker>();
                });
    }
}
