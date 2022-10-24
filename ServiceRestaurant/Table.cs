using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRestaurant
{
    public class Table
    {

        public State State { get; private set; }
        public int SeatsCount { get; }//количество мест
        public int Id { get; }
        public Table(int id)
        {
            Random rnd = new Random();
            State = State.Free;//столики при первом заказе свободные
            SeatsCount = rnd.Next(2,5);//мест от 2 до 5 рандомно
            Id = id;//id присваиваем при вызове
        }

        /// <summary>
        /// Статус бронирования
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool SetState(State state)
        {
            //State = state;
            if (state == this.State) return false;
            State = state;
            return true;
        }
    }
}
