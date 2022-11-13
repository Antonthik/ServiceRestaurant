
using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaraunt.Booking
{
    public class RestaurantBooking : SagaStateMachineInstance
    {
        //идентификатор для соотнесения всех сообщений друг с другом
        public Guid CorrelationId { get; set; }
        
        //текущее состояние саги(1-initial,2-final)
        public int CurrentState { get; set; }

        //идентификатор бронирования/заказа
        public Guid OrderId { get; set; }

        //идентификация клиента 
        public Guid ClientId { get; set; }

        //маркировка для "композиции событий"( случай с кухней и забронированным столом)
        public int ReadyEventStatus { get; set; }

        //пометка о том, что наша заявка просрочена
        public Guid? ExpirationId { get; set; }
    }
}
