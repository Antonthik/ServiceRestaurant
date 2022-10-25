using MassTransit;
using Restaurant.Messages;
using Restaurant.Notification.Consumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Kitchen
{
    /// <summary>
    /// Публикуем сообщения кухни о готовности заказа
    /// </summary>
    public class Manager
    {
        private readonly IBus _bus;

        public Manager(IBus bus)
        {
            _bus = bus;
        }

        /// <summary>
        /// Метод Издатель - публикация сообщений о готовности заказа со стороны кухни
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="dish"></param>
        public void CheckKitchenReady(Guid orderId, Dish? dish)
        {
            _bus.Publish<IKitchenReady>(new KitchenReady(orderId, true));
        }
    }
}
