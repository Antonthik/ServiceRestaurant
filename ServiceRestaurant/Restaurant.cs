using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRestaurant
{
    /// <summary>
    /// Генерим ресторан
    /// </summary>
    public class Restaurant
    {
        private readonly List<Table> _tables=new List<Table>();

        public Restaurant()
        {
            for(int i=0; i<=10; i++)
            {
                _tables.Add(new Table(i));//генерим 10 столов
            }
        }

        public void BookFreeTable(int countOfPerson)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу Вашу бронь, оставайтесь на линии");
            var table=_tables.FirstOrDefault(t=> t.SeatsCount>=countOfPerson && t.State==State.Free);

            Thread.Sleep(5000);//5 секунд в поисках стола
            if (table != null) table.SetState(State.Booked);//ставим бронь

            Console.WriteLine(table is null ? $"К сожалению, все столики заняты" : $"Готово!Ваш столик с номером {table.Id}");
        }

        public void FreeBookTable(int id)//снимаем бронь синхронно
        {
            Console.WriteLine($"Добрый день!Снимаем бронь со столика{id}");
            var table = _tables.FirstOrDefault(t => t.Id == id && t.State == State.Booked);

            Thread.Sleep(5000);//5 секунд в поисках стола
            if (table != null) table.SetState(State.Free);//снимаем бронь

            Console.WriteLine(table is null ? $"Столик не найден" : $"Готово!Бронь снята {table.Id}");

        }

        public void BookFreeTableAsync(int countOfPerson)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу Вашу бронь, оставайтесь на линии");

            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.SeatsCount >= countOfPerson && t.State == State.Free);

                await Task.Delay(100);//5 секунд в поисках стола

                if (table != null)
                {
                    if (table.SetState(State.Booked) == false)
                    { 
                        table = null;
                    }
                    Console.WriteLine(table is null ? $"Запрос отклонен, попробуйте позже..." : $"Готово!Ваш столик с номером {table.Id}");
                }
                else
                {                    
                    Console.WriteLine($"К сожалению, все столики заняты");
                } ;//ставим бронь

                
            });
        }
        public void FreeBookTableAsync(int id)
        {
            Console.WriteLine($"Добрый день!Снимаем бронь со столика{id}");

            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.Id == id && t.State == State.Booked);

                await Task.Delay(5000);//5 секунд в поисках стола
                if (table != null) table.SetState(State.Free);//снимаем бронь

                Console.WriteLine(table is null ? $"Столик не найден" : $"Готово!Бронь снята {table.Id}");
            });
        }

        public void FreeTablesAllAsync()
        {
            Task.Run(async () =>
            {
                for (int i = 0; i < _tables.Count; i++)
                {
                   await Task.Run(()=> _tables[i].SetState(State.Free));
                }

            });
        }

        public void DisplayTablesAll()
        {
            for (int i = 0; i < _tables.Count; i++)
            {
                Console.WriteLine($"Столик:{i} - {_tables[i].State}") ;
            }
        }
    }
}
