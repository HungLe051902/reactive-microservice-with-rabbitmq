using OrderService.Models;

namespace OrderService.Services.Interfaces
{
    public interface IOrderDetailsProvider
    {
        OrderDetail[] Get();
    }
}
