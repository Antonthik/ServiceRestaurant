using System.Diagnostics;

namespace ServiceRestaurant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var rest = new Restaurant();


            var Watch = new Stopwatch();
            Watch.Start();//замеряем время на бронирование

            while (true)
            {

                Console.WriteLine("Привет,желаете забронировать столик ?\n1 - Мы уведомим вас по SMS (асинхронно)" +
                    "\n2 - Подождите на линии мы вас уведомим (синхронно)");

                Console.WriteLine("Привет,желаете снять бронь?" +
                    "\n3 - Cнимем бронь(асинхронно)" +
                    "\n4 - Cнимем бронь (синхронно)");
                if (!int.TryParse(Console.ReadLine(),out var chose) && chose is not  (1 or 2 or 3 or 4 or 5))
                {
                    Console.WriteLine("Введите, пожалуста 1 или 2,3 или 4");
                    continue;
                };

                var stopWatch = new Stopwatch();
                stopWatch.Start();//замеряем время на бронирование

                Messages(rest, chose);

                Console.WriteLine("Спасибо за обращение.");
                stopWatch.Stop();//останавливаем время
                var ts=stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");

                if (Watch.ElapsedMilliseconds > 30000)
                {
                    
                    var ts0 = Watch.Elapsed;
                    Console.WriteLine($"{ts0.TotalMinutes:00}:{ts0.Seconds:00}:{ts0.Milliseconds:00}");
                    rest.FreeTablesAllAsync();
                    Watch.Stop();
                    Watch.Start();
                }
            }
        }


        static void Messages (Restaurant rest,int chose)
        {
            switch (chose)
            {
                case 1:
                    {
                        rest.BookFreeTableAsync(1);// бронь по смс
                        break;
                    }
                case 2:
                    {
                        rest.BookFreeTable(1);//бронь по звонку
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("Ведите номер столика.");
                        int.TryParse(Console.ReadLine(), out var id);
                        rest.FreeBookTableAsync(id);//снять бронь по смс
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("Ведите номер столика.");
                        int.TryParse(Console.ReadLine(), out var id);
                        rest.FreeBookTable(1);//снять бронь по звонку
                        break;
                    }
                case 5:
                    {
                        rest.DisplayTablesAll();
                        break;
                    }
            }

        }

        
}
}