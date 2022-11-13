using MassTransit;
using Restaurant.Booking;
using Restaurant.Messages;
using Restaurant.Messages.InMemoryDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaraunt.Booking.Consumers
{
    public class RestaurantBookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Restaurant _restaurant;
        private readonly IInMemoryRepository<BookingRequestModel> _repository;//репозиторий,через инъекцию зависимостей

        public RestaurantBookingRequestConsumer(Restaurant restaurant,
        IInMemoryRepository<BookingRequestModel> repository)
        {
            _restaurant = restaurant;
            _repository = repository;//репозиторий
        }

        //public async Task Consume(ConsumeContext<IBookingRequest> context)
        //{
        //    
        //    Console.WriteLine($"[OrderId: {context.Message.OrderId}]");
        //    var result = await _restaurant.BookFreeTableAsync(1);
        //    throw new AggregateException();//При обнаружении throw - Mass Transit отправит в очередь ошибок,что произошла ошибка
        //    await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        //}

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            var model = _repository.Get().FirstOrDefault(i => i.OrderId == context.Message.OrderId);

            if (model is not null && model.CheckMessageId(context.MessageId.ToString()))
            {
                Console.WriteLine(context.MessageId.ToString());
                Console.WriteLine("Second time");
                return;
            }

            var requestModel = new BookingRequestModel(context.Message.OrderId, context.Message.ClientId,
                context.Message.PreOrder, context.Message.CreationDate, context.MessageId.ToString());

            Console.WriteLine(context.MessageId.ToString());
            Console.WriteLine("First time");
            var resultModel = model?.Update(requestModel, context.MessageId.ToString()) ?? requestModel;

            _repository.AddOrUpdate(resultModel);
            var result = await _restaurant.BookFreeTableAsync(1);
            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        }

    }
}
