using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRestaurant
{
    public enum State
    {
        /// <summary>
        /// Столик свободен
        /// </summary>
        Free=0,

        /// <summary>
        /// Стол занят
        /// </summary>
        Booked=1
    }
}
