using MassTransit;
using Restaurant.Messages;

namespace Restaurant.Notification.Consumers
{
    //Каждый потребитель в MassTransit должен реализовывать интерфейс IConsumer<in T>, где T - тип сообщения.Р
    /// <summary>
    /// Принимаем сообщения от кухни и в общей очереди в заказе проставляем статус готовности кухни
    /// </summary>
    public class KitchenReadyConsumer : IConsumer<IKitchenReady>
        {
   
            private readonly Notifier _notifier;
   
            public KitchenReadyConsumer(Notifier notifier)
            {
                _notifier = notifier;
            }
        /// <summary>
        ///  Этот метод отвечает за получение сообщений от кухни и ставим статус кухни в заказе
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IKitchenReady> context)
            {
                _notifier.Accept(context.Message.OrderId, Accepted.Kitchen);
   
                return Task.CompletedTask;
            }
        }
    }
