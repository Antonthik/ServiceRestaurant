using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    /// <summary>
    /// Принимаем сообщения
    /// </summary>
    public class Consumer : IDisposable
    {
        private readonly string _queueName; //название очереди
        private readonly string _hostName; //хостнейм

        private readonly IConnection _connection;
        private readonly IModel _channel;

        /// <summary>
        /// Прослушиваем канал для получения сообщения
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="hostName"></param>
        public Consumer(string queueName, string hostName)
        {
            _queueName = queueName;
            _hostName = "rattlesnake.rmq.cloudamqp.com"; //hostName берем из Cluster;
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "yvkpdswf",//User & Vhost
                Password = "MEcnnefm_RCdTTZwHC8OBf-XLAs68IwL",
                VirtualHost = "yvkpdswf"//User & Vhost

            };
            _connection = factory.CreateConnection(); //создаем подключение
            _channel = _connection.CreateModel();
        }
        /// <summary>
        /// Обрабатываем полученное сообщение
        /// </summary>
        /// <param name="receiveCallback"></param>
        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
        {
            _channel.ExchangeDeclare(exchange: "direct_exchange",
                type: "direct"); // объявляем обменник- создает в брокере сам при запуске хостинга

            _channel.QueueDeclare(queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null); //объявляем очередь- создает в брокере сам при запуске хостинга

            _channel.QueueBind(queue: _queueName,
                exchange: "direct_exchange",
                routingKey: _queueName); //биндим- создает в брокере сам при запуске хостинга - между обменником и очередью

            var consumer = new EventingBasicConsumer(_channel); // создаем consumer для канала
            consumer.Received += receiveCallback; // добавляем обработчик события приема сообщения

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer); //стартуем! - autoAck: true -отправляем сообщение брокеру о том, что сообщение приняли
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}
