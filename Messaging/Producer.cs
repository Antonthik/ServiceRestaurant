using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    /// <summary>
    /// Отправка сообщения
    /// </summary>
    public class Producer
    {
        private readonly string _queueName;//имя очереди
        private readonly string _hostName;//строка подключения

        public Producer(string queueName, string hostName)
        {
            _queueName = queueName;
            _hostName = "rattlesnake.rmq.cloudamqp.com"; //hostName;
        }
        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            var factory = new ConnectionFactory()// объект подключения для облачного решения
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "yvkpdswf",
                Password = "MEcnnefm_RCdTTZwHC8OBf-XLAs68IwL",
                VirtualHost = "yvkpdswf"

            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();//настройка канала

            channel.ExchangeDeclare(
                "direct_exchange",//создаем точку end-point из которой отправляем сообщение
                "direct",//тип точки
                false,//сообщение в очереди если нет потребителя
                false,
                null
            );

            var body = Encoding.UTF8.GetBytes(message); // формируем тело сообщения для отправки

            channel.BasicPublish(exchange: "direct_exchange",//в созданный exchenge отправляем сообщение
                routingKey: _queueName,//какую очередь будет обрабатывать - в какой очереди будем обрабатывать сообщение
                basicProperties: null,
                body: body); //отправляем сообщение
        }
    }
}

