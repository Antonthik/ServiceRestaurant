using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaraunt.Booking
{
    /// <summary>
    /// Формируем столики в ресторане и реализуем метод отправки сообщения через продьюсера брокеру
    /// </summary>
    //public class Restaurant
    //{
    //    private readonly List<Table> _tables = new();
    //
    //    private readonly Producer _producer =
    //        new("BookingNotification", "localhost");
    //
    //    public Restaurant()
    //    {
    //        for (ushort i = 1; i <= 10; i++)
    //        {
    //            _tables.Add(new Table(i));
    //        }
    //    }
    //
    //    /// <summary>
    //    /// Метод отправки сообщений
    //    /// </summary>
    //    /// <param name="countOfPersons"></param>
    //    public void BookFreeTableAsync(int countOfPersons)
    //    {
    //        Console.WriteLine("Подождите секунду я подберу столик и подтвержу вашу бронь," +
    //                          "Вам придет уведомление");
    //        Task.Run(async () =>
    //        {
    //            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
    //                                                    && t.State == State.Free);
    //            await Task.Delay(1000 * 5); //у нас нерасторопные менеджеры, 5 секунд они находятся в поисках стола
    //            table?.SetState(State.Booked);
    //            
    //            _producer.Send(table is null//отправляем сообщение брокеру через продьюсера
    //                ? $"УВЕДОМЛЕНИЕ: К сожалению, сейчас все столики заняты"
    //                : $"УВЕДОМЛЕНИЕ: Готово! Ваш столик номер {table.Id}");
    //        });
    //    }
    //}

    
    public class Restaurant
    {
        private readonly List<Table> _tables = new();

        public Restaurant()
        {
            for (ushort i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        /// <summary>
        /// Метод для бронирования
        /// </summary>
        /// <param name="countOfPersons"></param>
        /// <returns></returns>
        public async Task<bool?> BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Спасибо за Ваше обращение, я подберу столик и подтвержу вашу бронь," +
                              "Вам придет уведомление");

            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                        && t.State == State.Free);
            await Task.Delay(1000 * 5); //у нас нерасторопные менеджеры, 5 секунд они находятся в поисках стола
            return table?.SetState(State.Booked);
        }
    }
}
