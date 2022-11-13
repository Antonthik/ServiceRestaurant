﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Messages
{
    //public interface ITableBooked
    //{
    //    public Guid OrderId { get; }
    //
    //    public Guid ClientId { get; }
    //
    //    public Dish? PreOrder { get; }
    //
    //    public bool Success { get; }
    //}

    public interface ITableBooked
    {
        public Guid OrderId { get; }

        public bool Success { get; }

        public DateTime CreationDate { get; }
    }
}
