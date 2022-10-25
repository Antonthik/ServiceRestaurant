using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Notification.Consumers;

namespace Restaurant.Notification
{
    /// <summary>
    /// Запуск хоста для прослушивания и приема сообщений
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();//запуск хоста для прослушивания сообщений
        }

        // public static IHostBuilder CreateHostBuilder(string[] args) =>
        //     Host.CreateDefaultBuilder(args)
        //         .ConfigureServices((hostContext, services) =>
        //         {
        //             services.AddHostedService<Worker>();
        //         });

       // .Notification - сервис отправки уведомлений, ожидает подтверждения от кухни и от сервиса бронирования;
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<NotifierTableBookedConsumer>();// опубликовать событие,что столик забронирован(TableBooked) и определить потребителя этого сообщения - NotifierTableBookedConsumer
                        x.AddConsumer<KitchenReadyConsumer>();//слушаем кухню

                        x.UsingRabbitMq((context, cfg) =>// указываем, что в качестве транспорта мы будем использовать rabbitMq
                        {
                            cfg.UseMessageRetry(r =>//конфигурация подключения
                            {
                                r.Exponential(5,
                                    TimeSpan.FromSeconds(1),
                                    TimeSpan.FromSeconds(100),
                                    TimeSpan.FromSeconds(5));
                                r.Ignore<StackOverflowException>();
                                r.Ignore<ArgumentNullException>(x => x.Message.Contains("Consumer"));
                            });


                            cfg.ConfigureEndpoints(context);
                        });



                    });
                    services.AddSingleton<Notifier>();
                    services.AddMassTransitHostedService(true);//в 8-ой версии MassTransit.RabbitMQ эта строка не нужна
                });
    }
}

