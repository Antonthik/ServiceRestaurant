using System.Collections.Concurrent;

namespace Restaurant.Notification
{
    /// <summary>
    /// Получаем сообщения-заказы и складываем в очередь, после акцепта или отклонения отчищаем эту очередь
    /// </summary>
    public class Notifier
    {
        //импровизированный кэш для хранения статусов, номера заказа и клиента
        private readonly ConcurrentDictionary<Guid, Tuple<Guid?, Accepted>> _state = new();

        /// <summary>
        /// Метод для акцепта
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="accepted"></param>
        /// <param name="clientId"></param>
        public void Accept(Guid orderId, Accepted accepted, Guid? clientId = null)
        {
            _state.AddOrUpdate(orderId, new Tuple<Guid?, Accepted>(clientId, accepted),
                (guid, oldValue) => new Tuple<Guid?, Accepted>(
                    oldValue.Item1 ?? clientId, oldValue.Item2 | accepted));

            Notify(orderId);
        }

        /// <summary>
        /// Метод для информирования о конечном статусе заказа
        /// </summary>
        /// <param name="orderId"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Notify(Guid orderId)
        {
            var booking = _state[orderId];

            switch (booking.Item2)
            {
                case Accepted.All:
                    Console.WriteLine($"Успешно забронировано для клиента {booking.Item1}");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.Rejected:
                    Console.WriteLine($"Гость {booking.Item1}, к сожалению, все столики заняты");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.Kitchen:
                case Accepted.Booking:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

  
}