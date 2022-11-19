﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaraunt.Booking
{
    //public class Table
    //{
    //    public State State { get; private set; }
    //    public int SeatsCount { get; }
    //    public int Id { get; }
    //
    //    private readonly object _lock = new object();
    //    private static readonly Random Random = new();
    //
    //    public Table(int id)
    //    {
    //        Id = id; //в учебном примере просто присвоим id при вызове
    //        State = State.Free; // новый стол всегда свободен
    //        SeatsCount = Random.Next(2, 5); //пусть количество мест за каждым столом будет случайным, от 2х до 5ти
    //    }
    //
    //    /// <summary>
    //    /// Установка занятости стола
    //    /// </summary>
    //    /// <param name="state"></param>
    //    /// <returns></returns>
    //    public bool SetState(State state)
    //    {
    //        lock (_lock)
    //        {
    //            if (state == State)
    //                return false;
    //
    //            State = state;
    //            return true;
    //        }
    //    }
    //
    //
    //
    //}
    public class Table
    {
        public TableState State { get; private set; }
        public int SeatsCount { get; }
        public int Id { get; }

        public Table(int id)
        {
            Id = id; //в учебном примере просто присвоим id при вызове
            State = TableState.Free; // новый стол всегда свободен
            SeatsCount = Random.Next(2, 5); //пусть количество мест за каждым столом будет случайным, от 2х до 5ти
        }

        public bool SetState(TableState state)
        {
            lock (_lock)
            {
                if (state == State)
                    return false;

                State = state;
                return true;
            }
        }

        private readonly object _lock = new object();
        private static readonly Random Random = new();

    }


}
