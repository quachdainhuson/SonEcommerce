using SonEcommerce.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SonEcommerce.Public.Orders
{
    public interface IOrdersAppService : ICrudAppService
        <OrderDto,
        Guid,
        PagedResultRequestDto, CreateOrderDto, CreateOrderDto>
    {
        Task<List<OrderInListDto>> GetListAllAsync();
        Task<OrderDto> GetOrderAndDetailsAsync(Guid orderId);
        // change status order
        Task ChangeStatusOrderAsync(Guid orderId, OrderStatus status);
        Task<List<OrderInListDto>> GetListOrderByUserIdAsync(Guid userId);
        // CancelOrderAsync
        Task<bool> CancelOrderAsync(Guid orderId);
    }
}
