
using Ecomm.DataAccess;
using Newtonsoft.Json;
using Plain.RabbitMQ;

namespace Ecomm
{
    public class ProductCreatedListener : IHostedService
    {
        private readonly IPublisher publisher;
        private readonly ISubscriber subscriber;
        private readonly IInventoryUpdator inventoryUpdator;

        public ProductCreatedListener(IPublisher publisher, ISubscriber subscriber, IInventoryUpdator inventoryUpdator)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.inventoryUpdator = inventoryUpdator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            var response = JsonConvert.DeserializeObject<OrderRequest>(message);
            if (!response!.IsSuccess)
            {
                orderDeletor.Delete(response.OrderId).GetAwaiter().GetResult();
            }
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class OrderRequest
    {
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
