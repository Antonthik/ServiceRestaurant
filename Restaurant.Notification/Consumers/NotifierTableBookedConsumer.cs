using MassTransit;
using Restaurant.Messages;

namespace Restaurant.Notification.Consumers
{

    /// <summary>
    /// Прослушиваем очередь для получения сообщений
    /// </summary>
    public class NotifierTableBookedConsumer : IConsumer<ITableBooked>
    {
        private readonly Notifier _notifier;
    
        public NotifierTableBookedConsumer(Notifier notifier)
        {
            _notifier = notifier;

        }
    
    /// <summary>
    /// Этот метод отвечает за получение сообщений из очереди
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task Consume(ConsumeContext<ITableBooked> context)
        {
            var result = context.Message.Success;

            _notifier.Accept(context.Message.OrderId, result ? Accepted.Booking : Accepted.Rejected,
                context.Message.OrderId);
    
            return Task.CompletedTask;
        }
    }
    public class BookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            Console.WriteLine($"[OrderId {context.Message.Message.OrderId}] Отмена в зале");
            return Task.CompletedTask;
        }
    }


}
